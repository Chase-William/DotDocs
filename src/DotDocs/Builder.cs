using System;
using System.Collections.Generic;
using DotDocs.Build;
using DotDocs.Build.Util;
using DotDocs.Render;
using DotDocs.Models;
using DotDocs.IO;
using DotDocs.Render;
using System.Collections.Immutable;

namespace DotDocs
{           
    /// <summary>
    /// The main class for using DotDoc's services.
    /// </summary>
    public class Builder
    {
        #region IO
        /// <summary>
        /// The location of repository.
        /// </summary>
        internal ISourceable Source { get; private set; }

        /// <summary>
        /// Output render results will be written to.
        /// </summary>
        internal IOutputable Output { get; private set; }
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
       
        public IRenderable Renderer { get; init; }

        private Builder(ISourceable _src, string _output, IRenderable renderable)
        {
            Source = _src;
            Output = new FileOutput(_output); // Change when other outputs are available
            Renderer = renderable;
        }

        /// <summary>
        /// Service from a url.
        /// </summary>
        /// <param name="url">Url to the github repository.</param>
        /// <param name="outDir">Location on disk to write render results to.</param>
        /// <returns></returns>
        public static Builder FromUrl(string url, string outDir, IRenderable renderable)
            => new(new GitCloneSource(url), outDir, renderable);

        /// <summary>
        /// Service from a path.
        /// </summary>
        /// <param name="path">Location of the repsitory root dir.</param>
        /// /// <param name="outDir">Location on disk to write render results to.</param>
        /// <returns></returns>
        public static Builder FromPath(string path, string outDir, IRenderable renderable)
            => new(new LocalSource(path), outDir, renderable);


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
                Output.Prepare();

                // Ensure output direct is valid before proceeding
                if (!Output.IsValid())                
                    throw new Exception($"Invalid Output: {Output}");                
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
                using Repository repo = new Repository(Source.Src)                    
                    .GetCommitInfo()
                    .MakeProjectGraph()
                    .SetActiveProject()
                    .EnableDocumentationGeneration()
                    .Build();
                
                // Apply built repository results to a model structure going top -> down
                // Assign results to respective properties via destructure
                var (repoModel, projs) = new RepositoryModel().Apply(repo);
                // Use RepositoryModel & Projects as data source for rendering
                // RepoModel & Projects dict share the same data,
                // they both providea  different perspective
                Renderer.Prepare(
                    repoModel, 
                    projs.ToImmutableDictionary(), 
                    Output);
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
    }
}
