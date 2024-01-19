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
            => Load(RootProjectBuildInstance, allAssemblyPaths);             

        public void Dispose()
        {
            foreach (var build in allProjectBuildInstances)
                build.Dispose();
        }        

                    
        /// <summary>
        /// Loads all local projects recursively.
        /// </summary>
        /// <param name="build">Project to start loading from.</param>
        /// <param name="assemblies">Assemblies that may be needed by <see cref="MetadataLoadContext"/>.</param>
        ProjectModel Load(ProjectBuildInstance build, ImmutableArray<string> assemblies)
        {
            ProjectModel? lowerModel = null;
            // Visit the lowest level assembly and load it's info before loading higher level assemblies
            foreach (var proj in build.DependentBuilds)
                lowerModel = Load(proj, assemblies); // Capture dependency projectModel
            var currentModel = build.Load(assemblies); // Capture this projectModel
            if (lowerModel != null) // If dependency exists, add it to the dependency list
                currentModel.Projects.Add(lowerModel);
            return currentModel;
        }
    }
}
