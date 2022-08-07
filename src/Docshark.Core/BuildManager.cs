using Docshark.Core.Exceptions;
using Docshark.Core.Models.Project;
using Docshark.Core.Tree;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Core
{
    public class BuildManager : IDisposable
    {
        /// <summary>
        /// The root folder of all file output produced by this project.
        /// </summary>
        public const string DOCSHARK_CORE_ROOT_FOLDER = "core-info";
        /// <summary>
        /// Folder that contains the project's namespace and type tree.
        /// </summary>
        public const string PROJECT_STRUCTURE_FOLDER = "projects";
        /// <summary>
        /// Folder that contains general metadata about the project.
        /// </summary>
        public const string META_FOLDER = "meta";

        /// <summary>
        /// A tree that organizes all types by their namespaces / encapsulating type if nested.
        /// </summary>
        // public ModelTree Models { get; private set; }

        /// <summary>
        /// A tree that contains all the local projects the root depends on.
        /// </summary>
        public ProjectTree ProjectManager { get; private set; }
        
        public TypeMetaMapper TypeMapper { get; private set; }        

        /// <summary>
        /// Path to .csproj.
        /// </summary>
        // public string ProjectFilePath { get; set; }

        string RootPath => Path.Combine(outputPath, DOCSHARK_CORE_ROOT_FOLDER);
        string ProjectStructureOutputDir => Path.Combine(RootPath, PROJECT_STRUCTURE_FOLDER);
        string MetadataPath => Path.Combine(RootPath, META_FOLDER);

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
        }

        public void Prepare()
        {
            ProjectManager = new ProjectTree();
            // Prepare all .csproj files recursively
            ProjectManager.Prepare(rootProjectFile);
            // Build the project
            ProjectManager.BuildProject(rootProjectFile);      
        }

        public void Load()
        {            
            TypeMapper = new TypeMetaMapper();
            ProjectManager.Load(ProjectManager.Assemblies, TypeMapper);          
        }

        public void Make()
        {
            ProjectManager.Save(ProjectStructureOutputDir);
            TypeMapper.SaveTypes(MetadataPath);                
        }    

        public void Dispose()
            => ProjectManager?.Dispose();        
    }
}
