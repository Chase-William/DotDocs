using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.IO.Compression;
using System.Linq;
using DotDocs.Core.Loader;
using DotDocs.Core.Loader.Services;
using MongoDB.Driver;

namespace DotDocs.Core
{
    /// <summary>
    /// The main class for using DotDoc's services.
    /// </summary>
    public class Builder
    {
        /// <summary>
        /// The root folder of all file output produced by this project.
        /// </summary>
        // public const string DOTDOCS_ROOT_FOLDER = "core-info";
        /// <summary>
        /// A tree that contains all the local projects the root depends on.
        /// </summary>
        // public ProjectBuilder repository;           
        /// <summary>
        /// The root path where DotDocs will output all documentation.
        /// </summary>
        // string RootPath => Path.Combine(outputPath, DOTDOCS_ROOT_FOLDER);
        /// <summary>
        /// The project file used as the root.
        /// </summary>
        string url;
        /// <summary>
        /// The output path provided by the user used in determining the root path for the documentation to reside.
        /// </summary>
        // string outputPath;
        IMongoDatabase commentDatabase;
        private CommentService comments;

        /// <summary>
        /// Instantiates a new instance of <see cref="Builder"/> which is ready to be used.
        /// </summary>
        /// <param name="csProjFile">The project file to be used as the root.</param>
        /// <param name="outputPath">The output path provided by the user used to determine where DotDocs should put rendered content.</param>
        public Builder(string url, IMongoDatabase commentDatabase)
        {
            this.url = url;
            this.commentDatabase = commentDatabase;
        }

        /// <summary>
        /// Prepares the <see cref="Builder"/> for loading information by modifing .csproj files where needed and building all projects to collect information.
        /// </summary>
        //public void Prepare()
        //{
        //    // Prepare all .csproj files recursively
        //    projectContext.Prepare(projectFile);
        //    // Build the project
        //    projectContext.BuildProject(projectFile);      
        //}

        /// <summary>
        /// Loads types from assemblies and documentation for all entities where available.
        /// </summary>
        //public void Load()
        //{
        //    projectContext.LoadTypes();
        //    projectContext.PrepareDocumentation();
        //}

        /// <summary>
        /// Cleans the output dir and renderers all documentation.
        /// </summary>
        public MemoryStream Document()
        {
            // https://github.com/Chase-William/.Docs.Core.git
            // comments = new CommentService(commentDatabase);
            // repository = new ProjectBuilder(commentManager);
            // repository = new ProjectLoadContext(commentManager);

            // Create a repository from the url
            // This 
            using Repository repo = new Repository(url, new CommentService(commentDatabase))
                .Download()
                .RetrieveHashInfo()
                .MakeProjectGraph()
                .SetActiveProject()
                .EnableDocumentationGeneration()
                .Build()
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
            var baseOutStream = new MemoryStream();
            var zip = new ZipArchive(baseOutStream, ZipArchiveMode.Create, true);                        
            // repository.Document(zip);
            // repository.Dispose();
            return baseOutStream;
        }                           

        /// <summary>
        /// Use to cleanup unmanaged resources used by the <see cref="repository"/>.
        /// </summary>
        //public void Dispose()
        //    => ProjectContext?.Dispose();        
    }
}
