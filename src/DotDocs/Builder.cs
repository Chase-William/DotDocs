using System;
using DotDocs.Build;
using DotDocs.Build.Util;
using DotDocs.Document;
using DotDocs.Models;
using DotDocs.Source;

namespace DotDocs
{           
    /// <summary>
    /// The main class for using DotDoc's services.
    /// </summary>
    public class Builder
    {
        internal ISourceable Source { get; private set; }

        internal RepositoryModel RepoModel { get; private set; }

        private Builder(ISourceable _src)
            => Source = _src;

        /// <summary>
        /// Service from a url.
        /// </summary>
        /// <param name="url">Url to the github repository.</param>
        /// <returns></returns>
        public static Builder FromUrl(string url)
            => new(new GitCloneSource(url));

        /// <summary>
        /// Service from a path.
        /// </summary>
        /// <param name="path">Location of the repsitory root dir.</param>
        /// <returns></returns>
        public static Builder FromPath(string path)
            => new(new LocalSource(path));


        public void Prepare()
        {
            try
            {
                // Prepare git source
                if (Source is GitCloneSource)
                    Source = Source.Prepare(); // Returns a new local source
                                               // Prepare local source
                _ = Source.Prepare();
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
                RepoModel = new RepositoryModel()
                    .Apply(repo);
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
                // Use RepositoryModel as data source for rendering
                var renderer = new Renderer(RepoModel);
                renderer.Render();                
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
