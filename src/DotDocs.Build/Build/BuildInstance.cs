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
    public class BuildInstance //: IDisposable
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
        public ProjectModel GetRootProject(
            Dictionary<string, ProjectModel> projects
            ) {
            // 
            //
            //  You remember ref assemblies? We need that for types so we can
            //  reference `int` later on as the return type of some method. This
            //  map should also probably hold user defined types as well. Therefore,
            //  it holds all types used *to an extent*.
            //
            //
            //
            var rootProject = Load(RootProjectBuildInstance, allAssemblyPaths, projects);
            // The root is not added until here
            projects.Add(rootProject.Name, rootProject);
            return rootProject;
        }           

        //public void Dispose()
        //{
        //    foreach (var build in allProjectBuildInstances)
        //        build.Dispose();
        //}

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

            build.InitMetadataLoadCtx(asmPaths);
            // Create a blank instance providing a target for dependencies to add themselves to
            var projModel = new ProjectModel(build.ProjectFileName, build.Assembly, build.MetadataLoadCtx);

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
                //.Apply(build.InitMetadataLoadCtx(asmPaths));
        }
    }
}
