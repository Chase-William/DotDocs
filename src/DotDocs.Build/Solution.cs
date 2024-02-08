using DotDocs.Build.Build;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;

namespace DotDocs.Build
{
    /// <summary>
    /// A class providing tools to interact and mutate a real repository and its project files on disk.
    /// </summary>
    public class Solution
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The results of the entire build.
        /// </summary>
        // internal BuildInstance? build;

        /// <summary>
        /// The directory of the solution.
        /// </summary>
        public string Dir { get; private set; }

        /// <summary>
        /// All project groups in the solution.
        /// </summary>
        public ImmutableArray<ProjectDocument> DependencyGraph { get; private set; }

        /// <summary>
        /// The select root project of a group to be documented.
        /// </summary>
        // public ProjectDocument? SelectedRootProject { get; private set; }
                    
        /// <summary>
        /// Creates a new solution.
        /// </summary>
        /// <param name="path">Location of the solution file.</param>
        private Solution(string path)
        {
            Logger.Debug("Params: [{pathLbl}: {pathValue}]", nameof(path), path);
            Dir = path;                    
        }

        public static Solution From(string path)
        {
            if (!Directory.Exists(path)) // Ensure path exists
                throw new DirectoryNotFoundException(path);

            var solution = new Solution(path);
            solution.MakeProjectGraph();

            return solution;
        }

        /// <summary>
        /// Creates a dependency graph for each project group.
        /// </summary>
        /// <returns></returns>
        private void MakeProjectGraph()
        {
            Logger.Trace("Making the project dependency graph.");
            
            // Locate all solution and project files            
            var projectFiles = Directory.GetFiles(Dir, "*.csproj", SearchOption.AllDirectories);
            DependencyGraph = FindRootProjects(projectFiles.ToList())
                .ToImmutableArray();
        }

        /// <summary>
        /// Builds active project via the property <see cref="SelectedRootProject"/>.
        /// </summary>
        /// <returns></returns>
        public static BuildInstance Build(ProjectDocument project)
        {
            Debug.Assert(project is not null);
            // Enable documentation generation on all project recursively
            Logger.Trace("Beginning recursive loop to enable documentation generation on all projects.");
            project.EnableAllDocumentationGeneration();

            return new BuildInstance(project)
                .Build();
        }        

        /// <summary>
        /// Returns all .csproj files that are the root project of a possibly larger project structure.
        /// </summary>
        /// <returns></returns>
        static IEnumerable<ProjectDocument> FindRootProjects(List<string> projectFiles)
        {
            var projects = new List<ProjectDocument>();

            while (projectFiles.Count != 0)
            {
                var proj = projectFiles.First();

                if (!File.Exists(projectFiles.First()))
                {
                    var ex = new FileNotFoundException($"The following project file path does not exist: {proj}");
                    Logger.Fatal(ex);
                    throw ex;
                }

                projects.Add(ProjectDocument.From(proj, projectFiles, projects));
            }
            return projects.Where(proj => proj.Parent == null).ToArray();
        }    
    }
}
