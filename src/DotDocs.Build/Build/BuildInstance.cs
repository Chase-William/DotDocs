using DotDocs.Build.Exceptions;
using DotDocs.Build.Util;
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
    public class BuildInstance : IDisposable
    {        
        ProjectDocument rootProject;

        public ProjectBuildInstance RootProjectBuildInstance { get; private set; }

        ImmutableArray<string> allAssemblyPaths;

        List<ProjectBuildInstance> allProjectBuildInstances = new();
        public ImmutableArray<ProjectBuildInstance> AllProjectBuildInstances 
            => allProjectBuildInstances.ToImmutableArray();

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

            try
            {
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
            }            
            catch (Exception ex)
            {
                Console.WriteLine();
            }

            return this;
        }

        /// <summary>
        /// Generates models for all of the projects, assemblies, and types.
        /// </summary>
        /// <returns>The root project.</returns>
        public ProjectModel MakeModels()
            => Load(RootProjectBuildInstance, allAssemblyPaths, new());             

        public void Dispose()
        {
            foreach (var build in allProjectBuildInstances)
                build.Dispose();
        }

        /// <summary>
        /// Recursive function that creates the project tree recursively in a BDF fashion.
        /// </summary>
        /// <param name="build">The build results for a project.</param>
        /// <param name="assemblies">All the assemblies needed by the application used by <see cref="MetadataLoadContext"/>.</param>
        /// <param name="models">A flat map of all project models created used to lookup and reference existing <see cref="ProjectModel"/>.</param>
        /// <returns></returns>
        ProjectModel Load(
            ProjectBuildInstance build,
            ImmutableArray<string> assemblies,
            Dictionary<string, ProjectModel> models
            ) {

            // Create a blank instance providing a target for dependencies to add themselves to
            var projModel = new ProjectModel();

            // Process nodes starting at the highest level leaf (furtherest into the tree)
            foreach (var proj in build.DependencyBuilds)
            {
                // This dependency ProjectModel has not been created yet
                if (!models.ContainsKey(proj.ProjectFileName))
                {
                    // Call this function recursively to reach highest level lead node
                    var dep = Load(proj, assemblies, models);
                    // Add dependency project to parent project list
                    projModel.Projects.Add(dep);
                    // Add to function's global map of project models
                    // used to prevent duplicate instances
                    models.Add(dep.Name, dep);                    
                    continue;
                }
                // ProjectModel already exists so get a reference
                // Note: This this proj must be a dependency to another proj then
                projModel.Projects.Add(models[proj.ProjectFileName]);
            }

            // Now that all dependency information for this project is setup
            // Initialize the MetadataLoadContext for reflection only introspection
            // Call the Apply method to apply assembmly info/build results to the model
            // Return to caller
            return projModel                
                .Apply(build.InitMetadataLoadCtx(assemblies));
        }
    }
}
