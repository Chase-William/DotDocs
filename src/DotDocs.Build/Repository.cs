using DotDocs.Build.Build;
using System.Collections.Immutable;
using System.Management.Automation;
using System.Reflection;

namespace DotDocs.Build
{
    public class Repository : IDisposable
    {
        internal BuildInstance build;

        public string Name { get; set; }
        /// <summary>
        /// The current commit hash of the repository.
        /// </summary>
        public string Commit { get; private set; }
        /// <summary>
        /// The url of the repository.
        /// </summary>
        public string Url { get; init; }

        /// <summary>
        /// The directory of the repository.
        /// </summary>
        public string Dir { get; private set; }

        // public ImmutableList<SolutionFile> Solutions { get; private set; }

        /// <summary>
        /// All project groups in the repository.
        /// </summary>
        public ImmutableArray<ProjectDocument> ProjectGraphs { get; private set; }

        /// <summary>
        /// The select root project of a group to be documented.
        /// </summary>
        public ProjectDocument ActiveProject { get; private set; }
                    

        public Repository(string path)
        {
            Dir = path;                    
        }        

        /// <summary>
        /// Retrieves the current hash for the HEAD commit of the downloaded repository.
        /// </summary>
        /// <param name="repoDir">Base directory of the repo.</param>
        /// <returns>Commit HEAD Hash</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public Repository GetCommitInfo()
        {
            string gitHeadFile = Path.Combine(Dir, @".git\HEAD");
            if (!File.Exists(gitHeadFile))
                throw new FileNotFoundException($"File 'HEAD' was not found at: {gitHeadFile}. Has the repository been downloaded using 'git clone <repo-url>' yet?");

            string commitHashFilePath = File.ReadAllText(gitHeadFile);
            // 'ref: ' <- skip these characters and get file dir that follows
            commitHashFilePath = Path.Combine(Dir, ".git", commitHashFilePath[5..]
                .Replace("\n", "")
                .Replace("/", "\\")
                .Trim());

            if (!File.Exists(commitHashFilePath))
                throw new FileNotFoundException($"The file containing the current HEAD file hash was not found at: {commitHashFilePath}");

            Commit = File.ReadAllText(commitHashFilePath)
                .Replace("\n", "")
                .Trim();
            return this;
        }

        /// <summary>
        /// Creates a dependency graph for each project group.
        /// </summary>
        /// <returns></returns>
        public Repository MakeProjectGraph()
        {
            // Locate all solution and project files            
            var projectFiles = Directory.GetFiles(Dir, "*.csproj", SearchOption.AllDirectories);
            ProjectGraphs = FindRootProjects(projectFiles.ToList())
                .ToImmutableArray();
            return this;
        }

        /// <summary>
        /// Ensures each project file has documentation generation enabled in the .csproj file.
        /// </summary>
        /// <returns></returns>
        public Repository EnableDocumentationGeneration()
        {
            ActiveProject.EnableDocumentationGeneration();
            return this;
        }

        /// <summary>
        /// Builds active project via the property <see cref="ActiveProject"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BuildException"></exception>
        public Repository Build()
        {
            build = new BuildInstance(ActiveProject)
                .Build();              

            return this;
        }

        /// <summary>
        /// Sets the active project to build.
        /// </summary>
        /// <returns></returns>
        public Repository SetActiveProject()
        {
            if (ProjectGraphs.Length > 1)
            {
                while (true)
                {
                    Console.WriteLine("Multiple related project groups detected. Please choose one:");
                    for (int i = 0; i < ProjectGraphs.Length; i++)
                        Console.WriteLine($"{i + 1} - {ProjectGraphs[i].ProjectFilePath}");
                    Console.Write(": ");
                    // Valid input
                    if (int.TryParse(Console.ReadLine(), out int index))
                    {
                        index--;
                        // Valid index range
                        if (index < ProjectGraphs.Length && index > -1)
                        {
                            ActiveProject = ProjectGraphs[index];
                            return this;
                        }
                    }
                }
            }
            ActiveProject = ProjectGraphs.First();
            return this;
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
                    throw new FileNotFoundException($"The following project file path does not exist: {proj}");

                projects.Add(ProjectDocument.From(proj, projectFiles, projects));
            }
            return projects.Where(proj => proj.Parent == null).ToArray();
        }

        public void Dispose()
        {
            // Deleted cloned repo            
            // Release metadataloadcontext'd assemblies
            build?.Dispose();

            // Delete repo from disk if it exists
            //if (Directory.Exists(Dir))
            //{
            //    // Using powershell because Directory.Delete recursive cannot delete some files for some reason.
            //    // .git's objects/pack/*.dix and *.pack files.. their not locked, just dont have access to the path
            //    // This is my work around below:
            //    using PowerShell powershell = PowerShell.Create();
            //    powershell.AddScript($"rm -r -fo {Dir}");
            //    powershell.Invoke(); // Run powershell            
            //}
        }        
    }
}
