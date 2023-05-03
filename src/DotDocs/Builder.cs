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

        internal Builder(string url)
        {
            this.url = url;
        }

        /// <summary>
        /// Performs the long runnning task of inserting the repository.
        /// </summary>
        public void AddRepository()
        {
            try
            {
                // Create a repository from the url
                // This 
                using Repository repo = new Repository(url)
                    .Download()
                    .GetCommitInfo()
                    .MakeProjectGraph()
                    .SetActiveProject()
                    .EnableDocumentationGeneration()
                    .Build();          

                RepositoryModel model = new RepositoryModel().Apply(repo);               
            
                model.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (Directory.Exists("downloads"))
                    Utility.CleanDirectory("downloads");
            }                       
        }
    }
}
