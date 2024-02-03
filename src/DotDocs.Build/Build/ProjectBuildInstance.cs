using DotDocs.Build.Util;
using DotDocs.Models;
using LoxSmoke.DocXml;
using Microsoft.Build.Logging.StructuredLogger;
using Microsoft.Build.Utilities;
using System.Collections.Immutable;
using System.Reflection;

namespace DotDocs.Build.Build
{
    /// <summary>
    /// A class containing a project's build information.
    /// </summary>
    public class ProjectBuildInstance
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

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

        /// <summary>
        /// The context used for loading in an assembly in a reflection-only manner.
        /// </summary>
        public MetadataLoadContext? MetadataLoadCtx { get; private set; }
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
        /// A collection of project builds that this project's build is dependent on.
        /// </summary>
        public ImmutableArray<ProjectBuildInstance> DependencyBuilds { get; set; }

        private ProjectBuildInstance() => Logger.Debug("Initializing");

        /// <summary>
        /// Create a new <see cref="ProjectBuildInstance"/> from the provided build evaluation
        /// and it's dependent builds.
        /// </summary>
        /// <param name="buildEval"></param>
        /// <param name="projects"></param>
        /// <returns></returns>
        public static ProjectBuildInstance From(ProjectEvaluation buildEval, List<ProjectBuildInstance> projects)
        {
            Logger.Trace("Creating a {projBuildInstance} from the given build evaluation of this project file: {buildEval}", typeof(ProjectBuildInstance).FullName, buildEval.ProjectFile);

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
                DependencyBuilds = GetDependencyBuilds(buildEval, projects)
            };
        }

        /// <summary>
        /// Initializes a new <see cref="MetadataLoadContext"/> instance for the current <see cref="ProjectBuildInstance"/> instance.
        /// </summary>
        /// <remarks>
        /// The <see cref="MetadataLoadContext"/> created will be used to inspect the assembly and its contents created from the build.
        /// </remarks>
        /// <param name="assemblies">Supporting assemblies that may be needed.</param>
        public void InitMetadataLoadCtx(ImmutableArray<string> assemblies)
        {
            Logger.Debug("Initializing a new {mlc} for this: {assembly}.", typeof(MetadataLoadContext).FullName, AssemblyFilePath);

            MetadataLoadCtx = new MetadataLoadContext(new PathAssemblyResolver(assemblies));
            Assembly = MetadataLoadCtx.LoadFromAssemblyPath(AssemblyFilePath);
        }

        /// <summary>
        /// Gets all dependencies of the given project evaluation; operates in a depth-first-search mode.
        /// </summary>
        /// <param name="projectEval">Current project.</param>
        /// <returns>All <see cref="ProjectModel"/> dependencies of the current.</returns>
        static ImmutableArray<ProjectBuildInstance> GetDependencyBuilds(ProjectEvaluation projectEval, List<ProjectBuildInstance> allProjects)
        {
            Logger.Trace("Getting all dependency builds for eval of project file: {projectFile}", projectEval.ProjectFile);

            var items = projectEval.FindChild<Folder>("Items");
            var addItems = items.FindChild<AddItem>("ProjectReference");
            if (addItems == null || addItems.Children.Count == 0)
                return new List<ProjectBuildInstance>().ToImmutableArray();

            IEnumerable<string> projectFileNames = addItems.Children
                .Cast<Item>()
                .Select(p => p.Text[(p.Text.LastIndexOf('\\') + 1)..]);

            var eval = (TimedNode)projectEval.Parent;
            var projects = new List<ProjectBuildInstance>();

            foreach (var projFileName in projectFileNames)
            {
                var projEval = eval.FindLastChild<ProjectEvaluation>(p => p.Name.Equals(projFileName));
                // Check to see if the project has already been loaded as a dependency elsewhere
                var existingProject = allProjects.SingleOrDefault(p => p.ProjectFileName.Equals(projFileName));
                if (existingProject != null) // Exist elsewhere so use existing instance
                    projects.Add(existingProject);
                else // Doesn't exist, create new
                {
                    var project = From(projEval, allProjects);
                    // Update both existing project list and add to tree
                    allProjects.Add(project);
                    projects.Add(project);
                }
            }
            return projects.ToImmutableArray();
        }
    }
}
