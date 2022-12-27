using System;
using System.IO;
using System.IO.Compression;
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
        public ProjectLoadContext ProjectContext { get; init; }             
        /// <summary>
        /// The root path where DotDocs will output all documentation.
        /// </summary>
        // string RootPath => Path.Combine(outputPath, DOTDOCS_ROOT_FOLDER);
        /// <summary>
        /// The project file used as the root.
        /// </summary>
        string projectFile;
        /// <summary>
        /// The output path provided by the user used in determining the root path for the documentation to reside.
        /// </summary>
        // string outputPath;

        /// <summary>
        /// Instantiates a new instance of <see cref="Builder"/> which is ready to be used.
        /// </summary>
        /// <param name="csProjFile">The project file to be used as the root.</param>
        /// <param name="outputPath">The output path provided by the user used to determine where DotDocs should put rendered content.</param>
        public Builder(string csProjFile, IMongoDatabase commentDatabase)
        {
            projectFile = csProjFile;
            // this.outputPath = outputPath;
            ProjectContext = new ProjectLoadContext(commentDatabase);
        }

        /// <summary>
        /// Prepares the <see cref="Builder"/> for loading information by modifing .csproj files where needed and building all projects to collect information.
        /// </summary>
        public void Prepare()
        {
            // Prepare all .csproj files recursively
            ProjectContext.Prepare(projectFile);
            // Build the project
            ProjectContext.BuildProject(projectFile);      
        }

        /// <summary>
        /// Loads types from assemblies and documentation for all entities where available.
        /// </summary>
        public void Load()
        {
            ProjectContext.LoadTypes();
            ProjectContext.PrepareDocumentation();
        }

        /// <summary>
        /// Cleans the output dir and renderers all documentation.
        /// </summary>
        public MemoryStream Document()
        {
            // Utility.CleanDirectory(output);
            var baseOutStream = new MemoryStream();
            var zip = new ZipArchive(baseOutStream, ZipArchiveMode.Create, true);                        
            ProjectContext.Document(zip);
            ProjectContext.Dispose();
            return baseOutStream;
        }    

        /// <summary>
        /// Use to cleanup unmanaged resources used by the <see cref="ProjectContext"/>.
        /// </summary>
        //public void Dispose()
        //    => ProjectContext?.Dispose();        
    }
}
