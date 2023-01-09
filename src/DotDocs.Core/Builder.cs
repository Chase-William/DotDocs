using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.IO.Compression;
using System.Linq;
using DotDocs.Core.Loader;
using DotDocs.Core.Loader.Services;
using DotDocs.Core.Models;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace DotDocs.Core
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

        IMongoDatabase commentDatabase;

        Configuration config;

        public Builder(string url, IMongoDatabase commentDatabase, string? config = null)
        {
            this.url = url;
            this.commentDatabase = commentDatabase;

            if (config == null)
                this.config = new Configuration(null, null, Perspective.Default);
            else
                this.config = JsonConvert.DeserializeObject<Configuration>(config).From();
        }

        /// <summary>
        /// Cleans the output dir and renderers all documentation.
        /// </summary>
        public MemoryStream Document()
        {
            // https://github.com/Chase-William/.Docs.Core.git
            // comments = new CommentService(commentDatabase);
            // repository = new ProjectBuilder(commentManager);
            // repository = new ProjectLoadContext(commentManager);

            //var baseOutStream = new MemoryStream();
            //var zip = new ZipArchive(baseOutStream, ZipArchiveMode.Create, true);

            // Create a repository from the url
            // This 
            using Repository repo = new Repository(url, new CommentService(commentDatabase), config)
                .Download()
                .RetrieveHashInfo()
                .MakeProjectGraph()
                .SetActiveProject()
                .EnableDocumentationGeneration()
                .Build()
                .Prepare()
                .Document();

            

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
            return null;
        }
    }
}
