using Docshark.Core.Global.Assemblies;
using Docshark.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Docshark.Core.Global.Projects
{
    /// <summary>
    /// Represents a .csproj and is referenced by <see cref="TypeDefinition"/>s.
    /// </summary>
    public class ProjectDefinition : Definition
    {
        /// <summary>
        /// The name of the project.
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// The location where the .csproj resides.
        /// </summary>
        public string ProjectDirectory { get; set; }
        /// <summary>
        /// The name of the project with its file extension.
        /// </summary>
        public string ProjectFileName { get; set; }
        /// <summary>
        /// A complete path to the project with the project file included.
        /// </summary>
        public string ProjectPath { get; set; }
        public string[] LocalProjectDependencies { get; set; }

        public static ProjectDefinition From(
            string projectName,
            string projectFileName,
            string projectDirectory,
            string projectPath,
            string[] projectDependencyNames,
            Assembly assembly,
            AssemblyMapper asmMapper)
        {
            ProjectDefinition proj = new()
            {
                ProjectName = projectName,                
                ProjectFileName = projectFileName,
                ProjectDirectory = projectDirectory,
                ProjectPath = projectPath,
                LocalProjectDependencies = projectDependencyNames
            };
            // Update the assembly this project produces to link back to this project using its key
            asmMapper.MappedDefinitions[assembly.GetPrimaryKey()].ProjectForeignKey = proj.GetPrimaryKey();
            return proj;
        }

        public override string GetPrimaryKey() 
            => ProjectName;

        internal static string GetPrimaryKeyMemberName()
            => nameof(ProjectName);
    }
}
