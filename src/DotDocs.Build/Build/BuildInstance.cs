using DotDocs.Build.Exceptions;
using DotDocs.Models;
using Microsoft.Build.Logging.StructuredLogger;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;

namespace DotDocs.Build.Build
{
    /// <summary>
    /// A class containing a build attempt's information.
    /// </summary>
    public class BuildInstance //: IDisposable
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The root project's document file i.e. (.csproj file).
        /// </summary>
        private readonly ProjectDocument rootProject;

        /// <summary>
        /// The root project i.e. (.csproj file) that all other projects support.
        /// </summary>
        public ProjectBuildInstance? RootProjectBuildInstance { get; private set; }
        /// <summary>
        /// All the assemblies used to support the compilation of this project.
        /// </summary>
        ImmutableArray<string> AllAssemblyPaths { get; set; } = new();
        /// <summary>
        /// All projects' build instances.
        /// </summary>
        List<ProjectBuildInstance> AllProjectBuildInstances { get; set; } = new();

        /// <summary>
        /// Creates an 
        /// </summary>
        /// <param name="rootProject"></param>
        public BuildInstance(ProjectDocument rootProject)
        {
            Logger.Debug("Params: [{rootProjectLbl}: {rootProjectValue}]", nameof(rootProject), rootProject);
            this.rootProject = rootProject;
        }

        /// <summary>
        /// Builds the root project via the <see cref="rootProject"/> property.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BuildException"></exception>
        public BuildInstance Build()
        {
            Logger.Trace("Building the solution.");

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

            try
            {
                Logger.Debug("Launching a {process} process.", cmd.StartInfo.FileName);
                cmd.Start();

                // Wait for files to finish being written & process close
                cmd.WaitForExit();
                var build = BinaryLog.ReadBuild("msbuild.binlog");
                // If the build fails throw exception with build info
                if (!build.Succeeded)
                    throw new BuildException(build.FindChildrenRecursive<Error>());

                var projectName = csProjPath[(csProjPath.LastIndexOf('\\') + 1)..];

                var mainBuild = build.FindLastChild<Project>();
                var target = mainBuild
                    .FindFirstChild<Target>(c => c.Name == "FindReferenceAssembliesForReferences");
                AllAssemblyPaths = target.Children
                    .Select(item => ((Item)((AddItem)item).FirstChild).Text)
                    .ToImmutableArray();

                // Get the root project
                var eval = build
                    .FindChild<TimedNode>("Evaluation");
                var projectEval = eval
                    .FindLastChild<ProjectEvaluation>(p => p.Name.Equals(projectName));

                RootProjectBuildInstance = ProjectBuildInstance
                    .From(projectEval, AllProjectBuildInstances);

                // Add the root node as it will never be added otherwise
                AllProjectBuildInstances.Add(RootProjectBuildInstance);
            }            
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                throw;
            }

            return this;
        }

        /// <summary>
        /// Generates models for all of the projects, assemblies, and types.
        /// </summary>
        /// <returns>The root project.</returns>
        public ImmutableArray<(string docs, Assembly binary)> GetAssemblies() {
            Logger.Trace("Getting the root project by recursively building up each project from the farthest leaf inward.");            

            var projects = new Dictionary<string, ProjectModel>();

            var rootProject = Load(
                RootProjectBuildInstance, 
                AllAssemblyPaths,
                projects);

            int index = 0;
            var assemblies = new (string docs, Assembly asm)[projects.Values.Count + 1]; // Leave room for the root project

            // Gather all assembles into collection
            foreach (var proj in projects.Values)
                assemblies[index++] = new(proj.DocumentationFilePath, proj.Assembly);
            assemblies[index] = new(rootProject.DocumentationFilePath, rootProject.Assembly);

            return assemblies.ToImmutableArray();
        }

        /// <summary>
        /// Recursive function that creates the project tree recursively in a BDF fashion.
        /// </summary>
        /// <param name="build">The build results for a project.</param>
        /// <param name="asmPaths">All the assemblies needed by the application used by <see cref="MetadataLoadContext"/>.</param>
        /// <param name="projects">A flat map of all project models created used to lookup and reference existing <see cref="ProjectModel"/>.</param>
        /// <returns></returns>
        ProjectModel Load(
            ProjectBuildInstance build,
            ImmutableArray<string> asmPaths,
            Dictionary<string, ProjectModel> projects
            ) {

            Logger.Trace("Processing project build: {buildProjectFile} with a project model count of {count}", build.ProjectFileName, projects.Count);
            
            build.InitMetadataLoadCtx(asmPaths);
            
            // Create a blank instance providing a target for dependencies to add themselves to
            var projModel = new ProjectModel(
                build.ProjectFileName, 
                build.Assembly,
                build.DocumentationFilePath,
                build.MetadataLoadCtx);

            // Process nodes starting at the highest level leaf (furtherest into the tree)
            foreach (var proj in build.DependencyBuilds)
            {
                // This dependency ProjectModel has not been created yet
                if (!projects.ContainsKey(proj.ProjectFileName))
                {
                    // Call this function recursively to reach highest level lead node
                    var dep = Load(proj, asmPaths, projects);
                    // Add dependency project to parent project list
                    projModel.Projects.Add(dep);
                    // Add to function's global map of project models
                    // used to prevent duplicate instances
                    projects.Add(dep.Name, dep);                    
                    continue;
                }
                // ProjectModel already exists so get a reference
                // Note: This this proj must be a dependency to another proj then
                projModel.Projects.Add(projects[proj.ProjectFileName]);
            }

            // Now that all dependency information for this project is setup
            // Initialize the MetadataLoadContext for reflection only introspection
            // Call the Apply method to apply assembmly info/build results to the model
            // Return to caller
            return projModel;
        }
    }
}
