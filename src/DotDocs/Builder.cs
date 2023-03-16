using DotDocs.Core;
using DotDocs.Core.Util;
using DotDocs.Models;
using Newtonsoft.Json;
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
        public async void Document()
        {
            try
            {
                // https://github.com/Chase-William/.Docs.Core.git
                // comments = new CommentService(commentDatabase);
                // repository = new ProjectBuilder(commentManager);
                // repository = new ProjectLoadContext(commentManager);

                //var baseOutStream = new MemoryStream();
                //var zip = new ZipArchive(baseOutStream, ZipArchiveMode.Create, true);

                // Create a repository from the url
                // This 
                using Repository repo = new Repository(url)
                    .Download()
                    .GetCommitInfo()
                    .MakeProjectGraph()
                    .SetActiveProject()
                    .EnableDocumentationGeneration()
                    .Build();
                //.Prepare()
                //.Document();            

                RepositoryModel model = new RepositoryModel().Apply(repo);

                GraphDatabaseConnection.Init(
                    "bolt://44.213.248.121:7687",
                    "neo4j",
                    "records-canyon-ditch");

                model.Run();

                //GraphDatabaseConnection.Close();

                // Take repo and return documentation


                // Returns the root node to all project structures
                // If there are multiple nodes here, we need to ask the user which tree to build for
                // Different trees are not related, therefore we do not build them into the documented output
                // var rootProjects = Utility.GetRootProjects(projectFiles.ToList());
                // Select project if multiple exists, otherwise the single existing project is selected.
                // ProjectDocument selectedProj = SelectProject(repo.ProjectGraphs);

                //repository
                //    .EnableDocumentationGeneration();


                // Prepare all .csproj files recursively
                //repository.Prepare("");
                //// Build the project
                //repository.BuildProject("");
                //// Load type info
                //repository.LoadTypes();
                //// Load documentation
                //repository.LoadDocumentation();

                // Utility.CleanDirectory(output);
                //var baseOutStream = new MemoryStream();
                //var zip = new ZipArchive(baseOutStream, ZipArchiveMode.Create, true);                        
                // repository.Document(zip);
                // repository.Dispose(); 
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (Directory.Exists("downloads"))
                    Utility.CleanDirectory("downloads");
            }                       
        }
    }
}
