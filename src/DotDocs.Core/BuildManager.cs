using System;
using System.IO;
using DotDocs.Core.Loader;

namespace DotDocs.Core
{
    public class BuildManager : IDisposable
    {
        /// <summary>
        /// The root folder of all file output produced by this project.
        /// </summary>
        public const string DOCSHARK_CORE_ROOT_FOLDER = "core-info";

        /// <summary>
        /// A tree that organizes all types by their namespaces / encapsulating type if nested.
        /// </summary>
        // public ModelTree Models { get; private set; }

        /// <summary>
        /// A tree that contains all the local projects the root depends on.
        /// </summary>
        public ProjectLoadContext ProjectContext { get; init; }
             

        /// <summary>
        /// Path to .csproj.
        /// </summary>
        // public string ProjectFilePath { get; set; }

        string RootPath => Path.Combine(outputPath, DOCSHARK_CORE_ROOT_FOLDER);

        /// <summary>
        /// Contains all reflection based metadata.
        /// </summary>
        // MetadataLoader metadata;

        string rootProjectFile;
        string outputPath;

        public BuildManager(string csProjFile, string outputPath)
        {
            rootProjectFile = csProjFile;
            this.outputPath = outputPath;
            ProjectContext = new ProjectLoadContext();
        }

        public void Prepare()
        {
            // Prepare all .csproj files recursively
            ProjectContext.Prepare(rootProjectFile);
            // Build the project
            ProjectContext.BuildProject(rootProjectFile);      
        }

        public void Load()
        {
            ProjectContext.LoadTypes();
            ProjectContext.LoadDocumentation();
        }

        public void Make()
        {
            Utility.CleanDirectory(RootPath);
            ProjectContext.Save(RootPath);             
        }    

        public void Dispose()
            => ProjectContext?.Dispose();        
    }
}
