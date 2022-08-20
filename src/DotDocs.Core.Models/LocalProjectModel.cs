using DotDocs.Core.Models.Language;
using System.Text.Json.Serialization;

namespace DotDocs.Core.Models
{
    /// <summary>
    /// Represents a local project with a .csproj file.
    /// </summary>
    public class LocalProjectModel
    {
        [JsonIgnore]
        /// <summary>
        /// Contains all the projects declared in this project.
        /// </summary>
        public List<TypeModel> DefinedTypes { get; } = new();
        /// <summary>
        /// Just the project name with no extension.
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// Directory where the .csproj file resides.
        /// </summary>
        public string ProjectDirectory { get; set; }
        /// <summary>
        /// Just the file name.
        /// </summary>
        public string ProjectFileName { get; set; }
        /// <summary>
        /// Entire path to file include name with extension.
        /// </summary>
        public string ProjectPath { get; set; }

        public string[] LocalProjects 
            => LocalProjectsAsObjects
            .Select(project => project.GetProjectId())
            .ToArray();

        public string AssemblyId { get; set; }

        public string Id => this.GetProjectId();

        [JsonIgnore]
        /// <summary>
        /// Collection of all <see cref="LocalProjectModel"/> dependencies.
        /// </summary>   
        public List<LocalProjectModel> LocalProjectsAsObjects { get; set; } = new();

        /// <summary>
        /// Determines if a projectFile
        /// Uses a depth-first-search (DFS) approach.
        /// </summary>
        /// <param name="projectName">Name of project to find.</param>
        /// <returns>A reference to the <see cref="LocalProjectModel"/> instance or null.</returns>
        public bool Exists(string projectFile)
        {
            foreach (var proj in LocalProjectsAsObjects)
            {
                if (proj.ProjectPath == projectFile) // base case
                    return true;
                return proj.Exists(projectFile);
            }
            return false;
        }
    }
}
