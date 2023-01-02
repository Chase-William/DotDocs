using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DotDocs.Core.Loader.Exceptions;
using DotDocs.Core.Loader.Services;
using DotDocs.Core.Models;
using DotDocs.Core.Models.Language;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Logging.StructuredLogger;

namespace DotDocs.Core.Loader
{
    public class Repository
    {
        /// <summary>
        /// The current commit hash of the repository.
        /// </summary>
        public string CommitHash { get; private set; }
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

        public Repository(string url)
            => Url = url;                

        /// <summary>
        /// Downloads a repository and returns the path to the repository.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Repository Download()
        {
            // CODE FOR DOWNLOADING AND BUILDNG
            string directory = AppContext.BaseDirectory; // directory of process execution
            string downloadRepoLocation = Path.Combine(directory, "downloads");
            if (!Directory.Exists(downloadRepoLocation))
                Directory.CreateDirectory(downloadRepoLocation);

            using PowerShell powershell = PowerShell.Create();
            // this changes from the user folder that PowerShell starts up with to your git repository
            powershell.AddScript($"cd {downloadRepoLocation}");
            powershell.AddScript(@"git clone https://github.com/Chase-William/.Docs.Core.git");
            //powershell.AddScript("cd.. / .. /.Docs.Core");
            //powershell.AddScript("dotnet build");
            powershell.Invoke(); // Run powershell            

            var folder = Url.Split("/").Last();
            if (folder.Contains(".git"))
                folder = folder[..4];

            Dir = Path.Combine(downloadRepoLocation, folder);
            return this;
        }

        /// <summary>
        /// Retrieves the current hash for the HEAD commit of the downloaded repository.
        /// </summary>
        /// <param name="repoDir">Base directory of the repo.</param>
        /// <returns>Commit HEAD Hash</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public Repository RetrieveHashInfo()
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

            CommitHash = File.ReadAllText(commitHashFilePath)
                .Replace("\n", "")
                .Trim();
            return this;
        }

        //public Repository FindSolutions()
        //{
        //    var solutionFiles = Directory.GetFiles(repoDir, "*.sln", SearchOption.AllDirectories);
            
        //}

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
            var instance = new BuildInstance(ActiveProject)
                .Build()
                .MakeModels();                

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
                            ActiveProject = ProjectGraphs[index];
                    }
                }
            }
            ActiveProject = ProjectGraphs.First();
            return this;
        }

        public Repository Document()
        {

            return this;
        }

        /// <summary>
        /// Returns all .csproj files that are the root project of a possibly larger project structure.
        /// </summary>
        /// <returns></returns>
        static IEnumerable<ProjectDocument> FindRootProjects(List<string> projectFiles)
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

        /// <summary>
        /// A class containing a build attempt's information.
        /// </summary>
        class BuildInstance : IDisposable
        {
            #region Binlog Variables
            const string PROJECT_NAME = "ProjectName";
            const string PROJECT_DIR = "ProjectDir";
            const string PROJECT_FILE_NAME = "ProjectFileName";
            const string PROJECT_PATH = "ProjectPath";
            const string TARGET_FILE_NAME = "TargetName";
            const string ASSEMBLY_NAME = "AssemblyName";
            const string TARGET_PATH = "TargetPath";
            const string DOCUMENTATION_FILE = "DocumentationFile";
            #endregion

            ProjectDocument rootProject;

            ProjectBuildInstance RootProjectBuildInstance { get; set; }

            ImmutableArray<string> allAssemblyPaths;

            List<ProjectBuildInstance> allProjectBuildInstances = new();

            public BuildInstance(ProjectDocument rootProject)
                => this.rootProject = rootProject;

            /// <summary>
            /// Builds the root project via the <see cref="rootProject"/> property.
            /// </summary>
            /// <returns></returns>
            /// <exception cref="BuildException"></exception>
            public BuildInstance Build()
            {
                var csProjPath = rootProject.ProjectFilePath;

                var cmd = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        Arguments = $"/C dotnet build {csProjPath} /bl"
                    }
                };
                cmd.Start();

                // Wait for files to finish being written & process close
                cmd.WaitForExit();
                var build = BinaryLog.ReadBuild("msbuild.binlog");
                // If the build fails throw exception with build info
                if (!build.Succeeded)
                    throw new BuildException(build.FindChildrenRecursive<Error>());

                var projectName = csProjPath[(csProjPath.LastIndexOf('\\') + 1)..];

                var mainBuild = build.FindLastChild<Microsoft.Build.Logging.StructuredLogger.Project>();
                var target = mainBuild
                    .FindFirstChild<Target>(c => c.Name == "FindReferenceAssembliesForReferences");
                allAssemblyPaths = target.Children
                    .Select(item => ((Item)((AddItem)item).FirstChild).Text)
                    .ToImmutableArray();

                // Get the root project
                var eval = build
                    .FindChild<TimedNode>("Evaluation");
                var projectEval = eval
                    .FindLastChild<ProjectEvaluation>(p => p.Name.Equals(projectName));

                RootProjectBuildInstance = ProjectBuildInstance
                    .From(projectEval, allProjectBuildInstances);

                // Add the root node as it will never be added otherwise
                allProjectBuildInstances.Add(RootProjectBuildInstance);

                return this;
            }

            public BuildInstance MakeModels()
            {
                Load(RootProjectBuildInstance, allAssemblyPaths);
                return this;
            }

            /// <summary>
            /// Loads all local projects recursively.
            /// </summary>
            /// <param name="build">Project to start loading from.</param>
            /// <param name="assemblies">Assemblies that may be needed by <see cref="MetadataLoadContext"/>.</param>
            void Load(ProjectBuildInstance build, ImmutableArray<string> assemblies)
            {
                // Visit the lowest level assembly and load it's info before loading higher level assemblies
                foreach (var proj in build.DependentBuilds)
                    Load(proj, assemblies);
                build.Load(assemblies);
            }

            public void Dispose()
            {
                foreach (var build in allProjectBuildInstances)
                    build.Dispose();
            }

            /// <summary>
            /// A class containing a project's build information.
            /// </summary>
            class ProjectBuildInstance : IDisposable
            {
                /// <summary>
                /// The context used for loading in an assembly in a reflection-only manner.
                /// </summary>
                MetadataLoadContext mlc;
                /// <summary>
                /// Name of the project file.
                /// </summary>
                public string ProjectFileName { get; private set; } = string.Empty;
                /// <summary>
                /// File location of the assembly.
                /// </summary>
                public string AssemblyFilePath { get; private set; } = string.Empty;
                /// <summary>
                /// File location of the generated documentation.
                /// </summary>
                public string DocumentationFilePath { get; private set; } = string.Empty;
                /// <summary>
                /// The assembly produced from the build.
                /// </summary>
                public Assembly? Assembly { get; private set; }
                /// <summary>
                /// Models of interest found within the <see cref="Assembly"/>.
                /// </summary>
                public ImmutableArray<TypeModel> Models { get; private set; }

                public ImmutableArray<ProjectBuildInstance> DependentBuilds { get; private set; }

                /// <summary>
                /// Create a new <see cref="ProjectBuildInstance"/> from the provided build evaluation
                /// and it's dependent builds.
                /// </summary>
                /// <param name="buildEval"></param>
                /// <param name="projects"></param>
                /// <returns></returns>
                public static ProjectBuildInstance From(ProjectEvaluation buildEval, List<ProjectBuildInstance> projects)
                {
                    var props = buildEval.FindChild<Folder>("Properties");
                    var properties = props.Children.Cast<NameValueNode>().Where(p =>
                    {
                        return p.Name switch
                        {
                            PROJECT_DIR or
                            PROJECT_FILE_NAME or
                            PROJECT_PATH or
                            TARGET_FILE_NAME or
                            TARGET_PATH or
                            PROJECT_NAME or
                            ASSEMBLY_NAME or
                            DOCUMENTATION_FILE => true,
                            _ => false,
                        };
                    })
                    .ToDictionary(p => p.Name);

                    string projDir = properties[PROJECT_DIR].Value;

                    return new ProjectBuildInstance
                    {
                        ProjectFileName = properties[PROJECT_FILE_NAME].Value,
                        AssemblyFilePath = properties[TARGET_PATH].Value,
                        DocumentationFilePath = Path.Combine(projDir, properties[DOCUMENTATION_FILE].Value),
                        DependentBuilds = GetDependentBuilds(buildEval)
                    };
                }

                /// <summary>
                /// Loads all the desired types from this assembly into the <see cref="Models"/> collection.
                /// </summary>
                /// <param name="assemblies">Supporting assemblies.</param>
                public void Load(ImmutableArray<string> assemblies)
                {
                    if (mlc != null)
                        Dispose();
                    mlc = new MetadataLoadContext(new PathAssemblyResolver(assemblies));
                    Assembly = mlc.LoadFromAssemblyPath(AssemblyFilePath);

                    /*
                     * Do not add all types unless they are relevant to the custom types created by the developer(s)
                     * or if they are public. All types used in some way by the developer(s')('s) types will be added
                     * to the type list. That said, if a type is public and available to be used by external libraries,
                     * ensure that type is accounted for regardless if it's compiler generated.
                     */
                    var typesOfInterest = Assembly.DefinedTypes
                        .Where(type => !type
                            .CustomAttributes
                            .Any(attr => attr.AttributeType.Name == typeof(CompilerGeneratedAttribute).Name) ||
                                type.IsPublic && !type.IsNestedFamORAssem);

                    var models = new List<TypeModel>();
                    foreach (var type in typesOfInterest)
                        models.Add(new TypeModel(type, true));
                    Models = models.ToImmutableArray();
                }

                /// <summary>
                /// Disposes of unmanaged resources for this build.
                /// </summary>
                public void Dispose()
                    => mlc?.Dispose();

                /// <summary>
                /// Gets all dependencies of the given project evaluation;
                /// operates in a depth-first-search mode.
                /// </summary>
                /// <param name="projectEval">Current project.</param>
                /// <returns>All <see cref="LocalProjectModel"/> dependencies of the current.</returns>
                static ImmutableArray<ProjectBuildInstance> GetDependentBuilds(ProjectEvaluation projectEval)
                {
                    var items = projectEval.FindChild<Folder>("Items");
                    var addItems = items.FindChild<AddItem>("ProjectReference");
                    if (addItems == null || addItems.Children.Count == 0)
                        return new ImmutableArray<ProjectBuildInstance>();

                    IEnumerable<string> projectFileNames = addItems.Children
                        .Cast<Item>()
                        .Select(p => p.Text[(p.Text.LastIndexOf('\\') + 1)..]);

                    var eval = (TimedNode)projectEval.Parent;
                    var projects = new List<ProjectBuildInstance>();

                    foreach (var projFileName in projectFileNames)
                    {
                        var projEval = eval.FindLastChild<ProjectEvaluation>(p => p.Name.Equals(projFileName));
                        // Check to see if the project has already been loaded as a dependency elsewhere
                        var existingProject = projects.SingleOrDefault(p => p.ProjectFileName.Equals(projFileName));
                        if (existingProject != null) // Exist elsewhere so use existing instance
                            projects.Add(existingProject);
                        else // Doesn't exist, create new
                        {
                            var project = From(projEval, projects);
                            // Update both existing project list and add to tree
                            projects.Add(project);
                            projects.Add(project);
                        }
                    }
                    return projects.ToImmutableArray();
                }               
            }
        }
    }
}
