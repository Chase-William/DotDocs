using DotDocs.Core;
using DotDocs.Core.Util;
using DotDocs.Models;
using System;
using System.IO;

namespace DotDocs
{
    /// <summary>
    /// The main class for using DotDoc's services.
    /// </summary>
    public class Builder
    {        
        /// <summary>
        /// The project file used as the root.
        /// </summary>
        string url;

        public Builder(string url)
        {
            this.url = url;
        }

        /// <summary>
        /// Cleans the output dir and renderers all documentation.
        /// </summary>
        public void Document()
        {
            try
            {
                // Create a repository from the url
                using Repository repo = new Repository(url)
                    .Download()
                    .GetCommitInfo()
                    .MakeProjectGraph()
                    .SetActiveProject()
                    .EnableDocumentationGeneration()
                    .Build();      

                // Create a more user-friendly representation of the repository's contents
                RepositoryModel model = new RepositoryModel().Apply(repo);
                PDF.PDF.Make(model);

            }            
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (Directory.Exists("downloads"))
                    Utility.CleanDirectory("downloads");
            }                       
        }
    }
}
