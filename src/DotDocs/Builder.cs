using DotDocs.Build;
using DotDocs.Build.Exceptions;
using DotDocs.IO;
using DotDocs.IO.Routing;
using DotDocs.Markdown;
using DotDocs.Models;
using DotDocs.Rendering;
using Microsoft.Build.Construction;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace DotDocs
{
    /// <summary>
    /// The main class for using DotDoc's services.
    /// </summary>
    public class Builder : IDisposable
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();               

        /// <summary>
        /// The renderer used to render all output.
        /// </summary>
        public IRenderer Renderer { get; init; }

        public string AbsoluteSolutionPath { get; init; }

        public string ProjectFileName { get; init; }

        ImmutableArray<AssemblyReflectInfo> assemblies = new();

        private Builder(
            string solutionPath,
            string projectFileName,
            IRenderer renderable)
        {
            AbsoluteSolutionPath = Path.GetFullPath(solutionPath);
            ProjectFileName = projectFileName;
            Renderer = renderable;
        }

        /// <summary>
        /// Creates a new <see cref="Builder"/> using the provided <see cref="IRenderer"/>.
        /// </summary>
        /// <param name="solutionPath">A path containing the solution file i.e. (.sln).</param>
        /// <param name="renderable">An implementation for rendering.</param>
        /// <returns></returns>
        public static Builder FromPath(
            string solutionPath,
            string projectFileName,
            IRenderer renderable)
            => new(solutionPath, projectFileName, renderable);

        /// <summary>
        /// Creates a new <see cref="Builder"/> using the default render options.
        /// </summary>
        /// <param name="solutionPath">A path containing the solution file i.e. (.sln).</param>
        /// <param name="outputPath">A path where render results shall be stored.</param>
        /// <returns></returns>
        public static Builder FromPath(string solutionPath, string projectFileName, string outputPath)
        {
            // var output = new LocalSource(solutionPath);
            var renderer = new MarkdownRenderer(
                new TextFileOutput(
                    outputPath, 
                    new FlatRouter(), 
                    ".md"));
            return new(solutionPath, projectFileName, renderer);
        }

        /// <summary>
        /// Starts build process and hands results to the renderer.
        /// </summary>
        public void Build()
        {
            try
            {
                var path = Path.GetFullPath(AbsoluteSolutionPath);
                // Parse solution
                var solution = SolutionFile.Parse(path);
                
                // Enable doc generation on all projects
                foreach (var proj in solution.ProjectsInOrder)
                    proj.EnableDocGen();

                // Get the specified project to build
                var source = solution.ProjectsInOrder.Single(p => ProjectFileName.StartsWith(p.ProjectName));
                // Build the project
                var build = source.Build();
                // If it didn't succeed, throw an error back indicating it was an issue with the project's content itself
                if (!build.Succeeded) // Throw build error exception              
                    throw new BuildException(build);                
                // Get all assemblies
                assemblies = build.GetAssemblies();
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
            foreach (var info in assemblies)            
                info.Dispose();            
        }
    }
}
