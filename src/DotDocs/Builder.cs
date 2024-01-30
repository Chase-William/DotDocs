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

namespace DotDocs
{           
    /// <summary>
    /// The main class for using DotDoc's services.
    /// </summary>
    public class Builder : IDisposable
    {
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

        public IRenderer Renderer { get; init; }
                

        private Builder(ISourceable _src, IRenderer renderable)
        {
            Source = _src;
            Renderer = renderable;
        }

        /// <summary>
        /// Service from a url.
        /// </summary>
        /// <param name="url">Url to the github repository.</param>
        /// <param name="outDir">Location on disk to write render results to.</param>
        /// <returns></returns>
        public static Builder FromUrl(string url, IRenderer renderable)
            => new(new GitCloneSource(url), renderable);

        /// <summary>
        /// Service from a path.
        /// </summary>
        /// <param name="path">Location of the repsitory root dir.</param>
        /// /// <param name="outDir">Location on disk to write render results to.</param>
        /// <returns></returns>
        public static Builder FromPath(string path, IRenderer renderable)
            => new(new LocalSource(path), renderable);


        public void Prepare()
        {
            try
            {
                // -- Prepare source

                // Prepare git source
                if (Source is GitCloneSource)
                    Source = Source.Prepare(); // Returns a new local source
                                               // Prepare local source
                _ = Source.Prepare();

                // -- Prepare output

                // Prepare output directory
                Renderer.Output.Clean();

                // Ensure output direct is valid before proceeding
                // if (!Output.IsValid())                
                    // throw new Exception($"Invalid Output: {Output}");                
            }
            catch
            {
                throw;
            }
            finally
            {
                //if (Directory.Exists("downloads"))
                //    Utility.CleanDirectory("downloads");
            }            
        }

        public void Build()
        {
            try
            {
                // Process repository files and build
                // Must close unmanaged MetadataLoadContext
                var repo = new Repository(Source.Src)                    
                    .GetCommitInfo()
                    .MakeProjectGraph()
                    .SetActiveProject()
                    .EnableDocumentationGeneration()
                    .Build();

                // Apply built repository results to a model structure going top -> down
                // Assign results to respective properties via destructure
                var projects = new Dictionary<string, ProjectModel>();
                var repoModel = new RepositoryModel().Apply(repo, projects);                         

                // Use RepositoryModel & Projects as data source for rendering
                // RepoModel & Projects dict share the same data,
                // they both providea  different perspective
                Renderer.Init(
                    repoModel, 
                    projects.ToImmutableDictionary());
            }
            catch
            {
                throw;
            }            
        }

        public void Document()
        {
            try
            {
                Renderer.Render();               
            }
            catch
            {
                
            }
            finally
            {
                //if (Directory.Exists("downloads"))
                //    Utility.CleanDirectory("downloads");
            }                       
        }

        public void Dispose()
        {
            if (Projects is not null)            
                foreach (var proj in Projects.Values)
                    proj.Dispose();            
        }
    }
}
