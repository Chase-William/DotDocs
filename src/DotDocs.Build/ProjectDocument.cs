using System.Xml.Linq;
using System.Xml.XPath;

namespace DotDocs.Build
{
    public class ProjectDocument
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The possible parent for this <see cref="ProjectDocument"/> instance.
        /// </summary>
        public ProjectDocument? Parent { get; private set; }
        /// <summary>
        /// A collection of <see cref="ProjectDocument"/> instances representing the projects this project depends on.
        /// </summary>
        public List<ProjectDocument> Dependencies { get; private set; } = new();
        /// <summary>
        /// A filepath to the actualy .csproj file.
        /// </summary>
        public string ProjectFilePath { get; init; }

        XDocument xDoc;

        private ProjectDocument(string projectFile)
        {
            Logger.Debug("Params: [{projectFileLbl}: {projectFileValue}]", nameof(projectFile), projectFile);   
            ProjectFilePath = projectFile;
            xDoc = XDocument.Parse(File.ReadAllText(projectFile));            
        }

        /// <summary>
        /// Mutates this project and its dependency projects recursively so that documentation files are generated during compilation.
        /// </summary>        
        public void EnableAllDocumentationGeneration()
        {
            Logger.Trace("Enabling documentation generation for project file: {projectfile}", ProjectFilePath);
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
            } // Close writer before processing child nodes
                        

            foreach (var dependency in Dependencies)
                dependency.EnableAllDocumentationGeneration();
        }

        /// <summary>
        /// Creates a <see cref="ProjectDocument"/> and all dependency documents as well recursively.
        /// </summary>
        /// <param name="projectFile">The current project file.</param>
        /// <param name="projectFiles">A list of project files remaining.</param>
        /// <returns>A <see cref="ProjectDocument"/> instance.</returns>
        public static ProjectDocument From(string projectFile, List<string> projectFiles, List<ProjectDocument> projects)
        {
            Logger.Trace("Params: [{projectFileLbl}: {projectFileValue}, {projectFilesLbl}, {projectFilesValue}: {projectFilesValue}, {projectsLbl}: {projectsValue}]",
                nameof(projectFile), projectFile,
                nameof(projectFiles), string.Join(',', projectFiles),
                nameof(projects), string.Join(',', projects.Select(p => p.ProjectFilePath)));

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
                        if (includePath is null)
                        {
                            var ex = new Exception($"No 'Include' attribute was found for the 'ProjectReference' in the .csproj file at: {projectFile}");
                            Logger.Fatal(ex);
                            throw ex;
                        }
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
