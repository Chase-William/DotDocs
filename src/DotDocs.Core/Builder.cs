using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using DotDocs.Core.Loader;
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
        public ProjectLoadContext projectContext;           
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
        private CommentService commentManager;

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
            commentManager = new CommentService(commentDatabase);
            projectContext = new ProjectLoadContext(commentManager);

            var repoDir = GitService.DownloadRepository(url);
            projectContext.GitHash = GitService.GetGitHeadHash(repoDir);

            // Locate all solution and project files
            var solutionFiles = Directory.GetFiles(repoDir, "*.sln", SearchOption.AllDirectories);
            var projectFiles = Directory.GetFiles(repoDir, "*.csproj", SearchOption.AllDirectories);

            // Returns the root node to all project structures
            // If there are multiple nodes here, we need to ask the user which tree to build for
            // Different trees are not related, therefore we do not build them into the documented output
            var rootProjects = Utility.GetRootProjects(projectFiles.ToList());
            // Select project if multiple exists, otherwise the single existing project is selected.
            ProjectDocument selectedProj = SelectProject(rootProjects);           

            // Prepare all .csproj files recursively
            projectContext.Prepare("");
            // Build the project
            projectContext.BuildProject("");
            // Load type info
            projectContext.LoadTypes();
            // Load documentation
            projectContext.LoadDocumentation();

            // Utility.CleanDirectory(output);
            var baseOutStream = new MemoryStream();
            var zip = new ZipArchive(baseOutStream, ZipArchiveMode.Create, true);                        
            projectContext.Document(zip);
            projectContext.Dispose();
            return baseOutStream;
        }    

        private ProjectDocument SelectProject(ProjectDocument[] rootProjects)
        {
            if (rootProjects.Length > 1)
            {
                while (true)
                {
                    Console.WriteLine("Multiple related project groups detected. Please choose one:");
                    for (int i = 0; i < rootProjects.Length; i++)
                        Console.WriteLine($"{i + 1} - {rootProjects[i].ProjectPath}");
                    Console.Write(": ");
                    // Valid input
                    if (int.TryParse(Console.ReadLine(), out int index))
                    {
                        index--;
                        // Valid index range
                        if (index < rootProjects.Length && index > -1)
                            return rootProjects[index];
                    }
                }
            }
            return rootProjects.First();
        }

        /// <summary>
        /// Use to cleanup unmanaged resources used by the <see cref="projectContext"/>.
        /// </summary>
        //public void Dispose()
        //    => ProjectContext?.Dispose();        
    }
}
