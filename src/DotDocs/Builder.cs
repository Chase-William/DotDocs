using System;
using System.Collections.Generic;
using DotDocs.Build;
using DotDocs.Build.Util;
using DotDocs.Render;
using DotDocs.Models;
using DotDocs.IO;
using System.Collections.Immutable;
using System.Linq;

using LoxSmoke.DocXml;
using System.Runtime.CompilerServices;

namespace DotDocs
{           
    /// <summary>
    /// The main class for using DotDoc's services.
    /// </summary>
    public class Builder : IDisposable
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        #region IO
        /// <summary>
        /// The location of repository.
        /// </summary>
        internal ISourceable Source { get; private set; }
        #endregion

        #region Data
        /// <summary>
        /// Repository model structure that mimics the real programmical structure.
        /// </summary>
        internal RepositoryModel RepoModel { get; private set; }
        /// <summary>
        /// A flat map containing all locally defined projects used.
        /// </summary>
        internal Dictionary<string, ProjectModel> Projects { get; private set; }
        #endregion

        /// <summary>
        /// The renderer used to render all output.
        /// </summary>
        public IRenderer Renderer { get; init; }
                
        private Builder(ISourceable _src, IRenderer renderable)
        {
            Source = _src;
            Renderer = renderable;
        }       

        /// <summary>
        /// Initializes a new instance of the <see cref="Builder"/> class using the <paramref name="path"/> and <paramref name="renderable"/>.
        /// </summary>
        /// <param name="path">Path to the root directory of the repository.</param>
        /// <param name="renderable">Takes an implementation of <see cref="IRenderer"/> used as the <see cref="Renderer"/>.</param>
        /// <returns></returns>
        public static Builder FromPath(string path, IRenderer renderable)
            => new(new LocalSource(path), renderable);

        /// <summary>
        /// Prepares the source material by ensuring validity of directories and that results from the last build are cleaned up.
        /// </summary>
        public void Prepare()
        {
            try
            {
                // Prepare local source
                Logger.Trace("Preparing {source}.", Source.ToString());
                _ = Source.Prepare();

                // -- Prepare output
                // Prepare output directory
                Logger.Trace("Cleaning output directory.");
                Renderer.Output.Clean();              
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                throw;
            }
        }

        /// <summary>
        /// Starts build process and hands results to the renderer.
        /// </summary>
        public void Build()
        {
            try
            {               
                // Process repository files and build
                var repo = new Repository(Source.Src)
                    .MakeProjectGraph()
                    .SetActiveProject()
                    .EnableDocumentationGeneration()
                    .Build();

                // Apply built repository results to a model structure going top -> down
                // Assign results to respective properties via destructure
                var projects = new Dictionary<string, ProjectModel>();                
                var repoModel = new RepositoryModel().Apply(repo, projects);

                // Use RepositoryModel & Projects as data source for rendering
                // RepoModel & Projects dict share the same data, but they both provide a different perspective                
                Renderer.Init(
                    repoModel, 
                    projects.ToImmutableDictionary());
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                throw;
            }            
        }

        /// <summary>
        /// Invokes the <see cref="Renderer"/> to begin rendering.
        /// </summary>
        public void Render()
        {
            try
            {
                Renderer.Render();               
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                throw;
            }              
        }

        /// <summary>
        /// Cleans up unmanaged resources once this <see cref="Builder"/> instance is no longer needed.
        /// </summary>
        public void Dispose()
        {
            Logger.Trace("Cleaning up all unused resources by projects.");
            if (Projects is not null)            
                foreach (var proj in Projects.Values)
                    proj.Dispose();            
        }
    }
}
