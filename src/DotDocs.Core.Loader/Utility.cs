using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace DotDocs.Core.Loader
{    
    /// <summary>
    /// Contains utility functionalities needed by this project.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Deletes all elements within a directory if it exists and ensures the directory given exist afterwards.
        /// </summary>
        /// <param name="pathToClean">Path to be cleaned.</param>
        public static void CleanDirectory(string pathToClean)
        {
            // First clean anything inside the dir
            if (Directory.Exists(pathToClean))
                Directory.Delete(pathToClean, true);
            Directory.CreateDirectory(pathToClean);
        }

        /// <summary>
        /// Returns all .csproj files that are the root project of a possibly larger project structure.
        /// </summary>
        /// <returns></returns>
        public static ProjectDocument[] GetRootProjects(List<string> projectFiles)
        {            
            List<ProjectDocument> projects = new List<ProjectDocument>();

            while (projectFiles.Count != 0)
            {
                var proj = projectFiles.First();

                if (!File.Exists(projectFiles.First()))
                    throw new FileNotFoundException($"The following project file path does not exist: {proj}");

                projects.Add(ProjectDocument.From(proj, projectFiles, projects));
            }
            return projects.Where(proj => proj.Parent == null).ToArray();            
        }
    }    

    public class ProjectDocument
    {
        public ProjectDocument? Parent { get; set; }        
        public List<ProjectDocument> Dependencies { get; set; } = new();
        public string ProjectPath { get; init; }

        XDocument xDoc;

        private ProjectDocument(string projectFile)
        {
            ProjectPath = projectFile;
            xDoc = XDocument.Parse(File.ReadAllText(projectFile));
        }

        /// <summary>
        /// Creates a project document with all referenced local projects created as well.
        /// </summary>
        /// <param name="projectFile"></param>
        /// <param name="projectFiles"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static ProjectDocument From(string projectFile, List<string> projectFiles, List<ProjectDocument> projects)
        {
            ProjectDocument proj = new(projectFile);
            projectFiles.Remove(projectFile);
            // Get local project references
            var itemGroups = proj.xDoc.XPathSelectElements("//ItemGroup");
            var itemGroup = itemGroups.LastOrDefault()?.Descendants();
            if (itemGroup != null)
                foreach (var item in itemGroup)
                    if (item.Name.LocalName == "ProjectReference")
                    {
                        string? includePath = item.Attribute(XName.Get("Include"))?.Value;
                        if (includePath == null)
                            throw new Exception($"No 'Include' attribute was found for the 'ProjectReference' in the .csproj file at: {projectFile}");
                        // Calculate relative path from this .csproj to the included one
                        string absolutePath = Path.GetFullPath(Path.Combine(projectFile[..projectFile.LastIndexOf('\\')], includePath));
                        var dependencyProj = projects.SingleOrDefault(p => p.ProjectPath == absolutePath);
                        if (dependencyProj == null)
                        { // Dependency project hasn't already been accounted for
                            dependencyProj = From(absolutePath, projectFiles, projects); // Create child project from file                                                        
                        }     
                        else
                        { // Dependency project has been accounted for already
                            projects.Remove(dependencyProj);
                            dependencyProj.Parent = proj;
                        }

                        proj.Dependencies.Add(dependencyProj); // Add child to parent
                        dependencyProj.Parent = proj; // Set parent to caller
                    }
            return proj;
        }
    }
}
