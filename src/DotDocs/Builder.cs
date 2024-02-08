using System;
using System.Collections.Generic;
using DotDocs.Build;
using DotDocs.Render;
using DotDocs.Models;
using DotDocs.IO;
using System.Collections.Immutable;
using System.Linq;

using LoxSmoke.DocXml;
using System.Runtime.CompilerServices;
using DotDocs.Markdown;
using DotDocs.IO.Routing;
using Microsoft.Build.Logging.StructuredLogger;
using System.Reflection;

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
        /// Creates a new <see cref="Builder"/> using the provided <see cref="IRenderer"/>.
        /// </summary>
        /// <param name="solutionPath">A path containing the solution file i.e. (.sln).</param>
        /// <param name="renderable">An implementation for rendering.</param>
        /// <returns></returns>
        public static Builder FromPath(string solutionPath, IRenderer renderable)
            => new(new LocalSource(solutionPath), renderable);

        /// <summary>
        /// Creates a new <see cref="Builder"/> using the default render options.
        /// </summary>
        /// <param name="solutionPath">A path containing the solution file i.e. (.sln).</param>
        /// <param name="outputPath">A path where render results shall be stored.</param>
        /// <returns></returns>
        public static Builder FromPath(string solutionPath, string outputPath)
        {
            var output = new LocalSource(solutionPath);
            var renderer = new MarkdownRenderer(
                new TextFileOutput(
                    outputPath, 
                    new FlatRouter(), 
                    ".md"));
            return new(output, renderer);
        }           

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
                // Build a solution from file complete with dependency graph
                var solution = Solution.From(Source.Src);
                // Prompt the user if nessessary for the root project if multiple clusters exist
                var root = PromptProjectSelection(solution.DependencyGraph);                
                // Build the solution
                var build = Solution.Build(root);
                // Get the assemblies created
                var assemblies = build.GetAssemblies();                
                // Provide them to the renderer
                Renderer.Init(assemblies);
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

        private static ProjectDocument PromptProjectSelection(ImmutableArray<ProjectDocument> projects)
        {
            if (projects.Length > 1)
            {
                Logger.Trace("More than one root project was found, user is being prompted.");
                while (true)
                {
                    Console.WriteLine("Multiple related project groups detected. Please choose one:");
                    for (int i = 0; i < projects.Length; i++)
                        Console.WriteLine($"{i + 1} - {projects[i].ProjectFilePath}");
                    Console.Write(": ");
                    // Valid input
                    if (int.TryParse(Console.ReadLine(), out int index))
                    {
                        index--;
                        // Valid index range
                        if (index < projects.Length && index > -1)
                        {
                            return projects[index];
                        }
                    }
                }
            }
            return projects.First();
        }
    }
}
