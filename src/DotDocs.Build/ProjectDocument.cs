using System.Xml.Linq;
using System.Xml.XPath;

namespace DotDocs.Build
{
    public class ProjectDocument
    {
        public ProjectDocument? Parent { get; set; }
        public List<ProjectDocument> Dependencies { get; set; } = new();
        public string ProjectFilePath { get; init; }

        XDocument xDoc;

        private ProjectDocument(string projectFile)
        {
            ProjectFilePath = projectFile;
            xDoc = XDocument.Parse(File.ReadAllText(projectFile));            
        }

        public void EnableDocumentationGeneration(bool recursive = true)
        {
            // Enable documenation generation on current project
            var propertyGroup = xDoc.XPathSelectElement("//PropertyGroup");
            var docGenProp = propertyGroup.Descendants().SingleOrDefault(prop => prop.Name.LocalName == "GenerateDocumentationFile");
            // Add to .csproj file if it doesn't exist (never was enabled or disabled)
            if (docGenProp == null)
                propertyGroup.Add(new XElement("GenerateDocumentationFile")
                {
                    Value = "True"
                });
            // Update to true if set to false
            else if (bool.Parse(docGenProp.Value.ToLower()) == false)
                docGenProp.Value = "True";

            using (var writer = new StreamWriter(ProjectFilePath))
            {
                xDoc.Save(writer);
            }
                        

            if (recursive)
                foreach (var dependency in Dependencies)
                    dependency.EnableDocumentationGeneration();
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
                        var dependencyProj = projects.SingleOrDefault(p => p.ProjectFilePath == absolutePath);
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
