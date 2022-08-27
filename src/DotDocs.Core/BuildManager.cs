using System;
using System.IO;
using DotDocs.Core.Loader;

namespace DotDocs.Core
{
    /// <summary>
    /// The main class for using DotDoc's services.
    /// </summary>
    public class BuildManager : IDisposable
    {
        /// <summary>
        /// The root folder of all file output produced by this project.
        /// </summary>
        public const string DOTDOCS_ROOT_FOLDER = "core-info";
        /// <summary>
        /// A tree that contains all the local projects the root depends on.
        /// </summary>
        public ProjectLoadContext ProjectContext { get; init; }             
        /// <summary>
        /// The root path where DotDocs will output all documentation.
        /// </summary>
        string RootPath => Path.Combine(outputPath, DOTDOCS_ROOT_FOLDER);
        /// <summary>
        /// The project file used as the root.
        /// </summary>
        string rootProjectFile;
        /// <summary>
        /// The output path provided by the user used in determining the root path for the documentation to reside.
        /// </summary>
        string outputPath;

        /// <summary>
        /// Instantiates a new instance of <see cref="BuildManager"/> which is ready to be used.
        /// </summary>
        /// <param name="csProjFile">The project file to be used as the root.</param>
        /// <param name="outputPath">The output path provided by the user used to determine where DotDocs should put rendered content.</param>
        public BuildManager(string csProjFile, string outputPath)
        {
            rootProjectFile = csProjFile;
            this.outputPath = outputPath;
            ProjectContext = new ProjectLoadContext();
        }

        /// <summary>
        /// Prepares the <see cref="BuildManager"/> for loading information by modifing .csproj files where needed and building all projects to collect information.
        /// </summary>
        public void Prepare()
        {
            // Prepare all .csproj files recursively
            ProjectContext.Prepare(rootProjectFile);
            // Build the project
            ProjectContext.BuildProject(rootProjectFile);      
        }

        /// <summary>
        /// Loads types from assemblies and documentation for all entities where available.
        /// </summary>
        public void Load()
        {
            ProjectContext.LoadTypes();
            ProjectContext.LoadDocumentation();
        }

        /// <summary>
        /// Cleans the output dir and renderers all documentation.
        /// </summary>
        public void Render()
        {
            Utility.CleanDirectory(RootPath);
            ProjectContext.Save(RootPath);             
        }    

        /// <summary>
        /// Use to cleanup unmanaged resources used by the <see cref="ProjectContext"/>.
        /// </summary>
        public void Dispose()
            => ProjectContext?.Dispose();        
    }
}
