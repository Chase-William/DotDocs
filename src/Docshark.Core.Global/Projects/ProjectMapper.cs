using Docshark.Core.Global.Assemblies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Core.Global.Projects
{
    public class ProjectMapper : IMapper<ProjectDefinition>
    {
        public const string PROJECT_MAPPER_FILENAME = "projects.json";

        public IReadOnlyDictionary<string, ProjectDefinition> MappedDefinitions => mappedDefinitions;

        Dictionary<string, ProjectDefinition> mappedDefinitions = new();

        AssemblyMapper asmMapper;

        public ProjectMapper(AssemblyMapper asmMapper)
            => this.asmMapper = asmMapper;        

        public void AddProject(
            string projectName, 
            string projectFileName, 
            string projectDirectory, 
            string projectPath,
            string[] projectDependencyNames,
            Assembly assembly)
        {
            // Only create the new project and add it if it doesn't already exist
            if (!MappedDefinitions.ContainsKey(projectName))
                mappedDefinitions.Add(
                    projectName, 
                    ProjectDefinition.From(
                        projectName,
                        projectFileName,
                        projectDirectory,
                        projectPath,
                        projectDependencyNames,
                        assembly,
                        asmMapper));
        }
    }
}
