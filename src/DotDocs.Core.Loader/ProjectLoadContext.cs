using Microsoft.Build.Logging.StructuredLogger;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using DotDocs.Core.Models.Language;
using System.Text.Json;
using DotDocs.Core.Models;
using DotDocs.Core.Loader.Exceptions;

namespace DotDocs.Core.Loader
{
    public class ProjectLoadContext : IDisposable
    {
        /// <summary>
        /// Folder that contains the project's namespace and type tree.
        /// </summary>
        public const string PROJECTS_FILE = "projects.json";
        public const string TYPES_FILE = "types.json";
        public const string ASSEMBLIES_FILE = "assemblies.json";

        const string PROJECT_NAME = "ProjectName";
        const string PROJECT_DIR = "ProjectDir";
        const string PROJECT_FILE_NAME = "ProjectFileName";
        const string PROJECT_PATH = "ProjectPath";
        const string TARGET_FILE_NAME = "TargetName";
        const string ASSEMBLY_NAME = "AssemblyName";
        const string TARGET_PATH = "TargetPath";
        const string DOCUMENTATION_FILE = "DocumentationFile";        

        private List<LocalProjectContext> localProjects = new();
        public List<LocalProjectModel> LocalProjects => localProjects.ToList<LocalProjectModel>();

        LocalProjectContext rootProject;
        public LocalProjectModel RootProject => rootProject;

        Dictionary<string, TypeModel> fullProjectTypeMap = new();
        public IReadOnlyDictionary<string, TypeModel> FullProjectTypeMap => fullProjectTypeMap;

        Dictionary<string, AssemblyModel> assemblies = new();
        public IReadOnlyDictionary<string, AssemblyModel> Assemblies => assemblies;

        /// <summary>
        /// Assemblies that we want to capture member information from. Other types declared outside
        /// of this assembly we will not capture.
        /// </summary>
        private HashSet<Assembly> assembliesOfInterest;

        private string[] assembliesPaths { get; set; }

        public ProjectLoadContext() { }

        
        public void Save(string outputPath)
        {
            // Save assemblies
            using (var writer = new StreamWriter(Path.Combine(outputPath, ASSEMBLIES_FILE)))
                writer.Write(JsonSerializer.Serialize(Assemblies.Values));
            // Save projects
            using (var writer = new StreamWriter(Path.Combine(outputPath, PROJECTS_FILE)))            
                writer.Write(JsonSerializer.Serialize(LocalProjects));            
            // Save types
            using (var writer = new StreamWriter(Path.Combine(outputPath, TYPES_FILE)))            
                writer.Write(JsonSerializer.Serialize(FullProjectTypeMap.Values));                        
        }

        /// <summary>
        /// Prepares given and all dependent .csproj files recursively for further processing.
        /// </summary>
        /// <param name="projectFile">Current project file.</param>
        /// <param name="processedFiles">Already processed files.</param>
        /// <exception cref="MissingProjectFileException">Current .csproj file was not found.</exception>
        public void Prepare(string projectFile, List<string> processedFiles = null)
        {
            processedFiles ??= new();
            // Critial error if a required .csproj file cannot be found
            if (!File.Exists(projectFile))
                throw new MissingProjectFileException(projectFile);

            {
                string projFileNameWithEx = projectFile.Substring(projectFile.LastIndexOf("\\") + 1);
                // Check the already processed files to prevent re-processing of dependent local projects
                if (processedFiles.Contains(projFileNameWithEx))
                    return;

                // Add the projectFile to the list to signify it's processing has or will be handled
                processedFiles.Add(projFileNameWithEx);
            }

            // Get .csproj file into mem
            var docFile = XDocument.Parse(File.ReadAllText(projectFile));

            // Enable documenation generation on current project
            var propertyGroup = docFile.XPathSelectElement("//PropertyGroup");
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

            // Process child project files
            var itemGroups = docFile.XPathSelectElements("//ItemGroup");
            var itemGroup = itemGroups.LastOrDefault()?.Descendants();
            if (itemGroup != null)
                foreach (var item in itemGroup)
                    if (item.Name.LocalName == "ProjectReference")
                        Prepare(Path.Combine(projectFile[..projectFile.LastIndexOf('\\')], item.FirstAttribute.Value), processedFiles);

            using var stream = File.Open(projectFile, FileMode.Create);
            docFile.Save(stream);
        }


        /*
         * 
         * Gather all .dlls for each assembly.
         * 
         */
        public void BuildProject(string csProjPath)
        {
            try
            {
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

                var mainBuild = build.FindLastChild<Project>();
                var target = mainBuild
                    .FindFirstChild<Target>(c => c.Name == "FindReferenceAssembliesForReferences");
                assembliesPaths = target.Children.Select(item => ((Item)((AddItem)item).FirstChild).Text).ToArray();

                // Get the root project
                var eval = build
                    .FindChild<TimedNode>("Evaluation");
                var projectEval = eval
                    .FindLastChild<ProjectEvaluation>(p => p.Name.Equals(projectName));

                rootProject = GetLocalProjectInfo(projectEval);
                // Add the root node as it will never be added otherwise
                localProjects.Add(rootProject);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void LoadTypes()
        {            
            LoadRecursive(rootProject, assembliesPaths);
            assembliesOfInterest = localProjects.Select(proj => proj.Assembly).ToHashSet();
            // Add types and their type dependencies to the collection of all types
            foreach (var project in localProjects)
            {
                foreach (var type in project.DefinedTypes)
                {
                    AddType(type.Type);                    
                }
            }            
        }

        /// <summary>
        /// Disposes all <see cref="LocalProjectModel"/> within <see cref="rootProject"/> recursively.
        /// </summary>
        public void Dispose()
        {
            if (rootProject != null)
                DisposeNext(rootProject);
            static void DisposeNext(LocalProjectContext project)
            {
                foreach (var proj in project.LocalProjectsAsObjects)
                    DisposeNext((LocalProjectContext)proj);
                project.Dispose();
            }
        }

        /// <summary>
        /// Gets the local project's information and calls <see cref="GetLocalProjects(ProjectEvaluation)"/> to recursively aquire
        /// all sub projects too.
        /// </summary>
        /// <param name="projectEval">The project's build evaluation.</param>
        /// <returns>An instance to the <see cref="LocalProjectModel"/>.</returns>
        LocalProjectContext GetLocalProjectInfo(ProjectEvaluation projectEval)
        {
            var props = projectEval.FindChild<Folder>("Properties");
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

            return new LocalProjectContext
            {
                AssemblyId = properties[ASSEMBLY_NAME].Value,
                ProjectName = properties[PROJECT_NAME].Value,
                ProjectDirectory = projDir,
                ProjectFileName = properties[PROJECT_FILE_NAME].Value,
                ProjectPath = properties[PROJECT_PATH].Value,
                AssemblyLoadPath = properties[TARGET_PATH].Value,
                DocumentationPath = Path.Combine(projDir, properties[DOCUMENTATION_FILE].Value),
                LocalProjectsAsObjects = GetLocalProjects(projectEval).ToList<LocalProjectModel>()
            };
        }

        /// <summary>
        /// Gets all <see cref="LocalProject"/> dependencies of the given project evaluation.
        /// Operates in a depth-first-search mode.
        /// </summary>
        /// <param name="projectEval">Current project.</param>
        /// <returns>All <see cref="LocalProjectModel"/> dependencies of the current.</returns>
        List<LocalProjectContext> GetLocalProjects(ProjectEvaluation projectEval)
        {
            var items = projectEval.FindChild<Folder>("Items");
            var addItems = items.FindChild<AddItem>("ProjectReference");
            if (addItems == null || addItems.Children.Count == 0)
                return new List<LocalProjectContext>();

            IEnumerable<string> projectFileNames = addItems.Children.Cast<Item>().Select(p => p.Text[(p.Text.LastIndexOf('\\') + 1)..]);
            var eval = (TimedNode)projectEval.Parent;

            var projects = new List<LocalProjectContext>();

            foreach (var projFileName in projectFileNames)
            {
                var projEval = eval.FindLastChild<ProjectEvaluation>(p => p.Name.Equals(projFileName));
                // Check to see if the project has already been loaded as a dependency elsewhere
                var existingProject = localProjects.SingleOrDefault(p => p.ProjectFileName.Equals(projFileName));
                if (existingProject != null) // Exist elsewhere so use existing instance
                    projects.Add(existingProject);
                else // Doesn't exist, create new
                {
                    var project = GetLocalProjectInfo(projEval);
                    // Update both existing project list and add to tree
                    localProjects.Add(project);
                    projects.Add(project);
                }
            }
            return projects;
        }

        void LoadRecursive(LocalProjectContext project, string[] assemblies)
        {            
            // Visit the lowest level assembly and load it's info before loading higher level assemblies
            foreach (var proj in project.LocalProjectsAsObjects)
                LoadRecursive((LocalProjectContext)proj, assemblies);
            project.Load(assemblies);            
        }

        void AddType(Type type)
        {
            // Do not add if already accounted for
            if (fullProjectTypeMap.ContainsKey(type.GetTypeId()))
                return;

            // Add this model's type
            var model = new TypeModel(type, !assembliesOfInterest.Contains(type.Assembly));
            fullProjectTypeMap.Add(type.GetTypeId(), model);
            // Add assembly if needed and not already added
            AddAssembly(model);

            // There is a chance this type is a TypeInfo
            // try to cast it.
            var meta = type as TypeInfo;
            // If cast failed, get TypeInfo manually
            meta ??= type.GetTypeInfo();

            // Ensure all generic parameters are accounted for
            if (type.ContainsGenericParameters)
            {
                // Process generic parameters
                AddTypeParameters(meta.GenericTypeParameters);
            }

            // Ensure all type argument types are accounted for
            if (type.GenericTypeArguments.Length > 0)
                AddTypeArguments(type.GenericTypeArguments);

            // Ensure the base type is added too
            if (type.BaseType != null)
                AddType(type.BaseType);

            // Only pull member info from types defined in assemblies created locally from a local project
            if (!model.IsDefinedInLocalProject)
            {
                AddTypeInfo(
                    model.Properties.Select(prop => prop.Info),
                    model.Fields.Select(field => field.Info),
                    model.Methods.Select(method => method.Info),
                    model.Events.Select(_event => _event.Info));
            }
        }

        void AddTypeInfo(
            IEnumerable<PropertyInfo> properties,
            IEnumerable<FieldInfo> fields,
            IEnumerable<MethodInfo> methods,
            IEnumerable<EventInfo> events)
        {            
            // Add properties
            foreach (var property in properties)
                AddType(property.PropertyType);
            // Add fields
            foreach (var field in fields)            
                AddType(field.FieldType);            
            // Add methods
            foreach (var method in methods)
            {
                AddType(method.ReturnType);
                var parameters = method.GetParameters();
                foreach (var parameter in parameters)
                    AddType(parameter.ParameterType);
            }
            // Add events
            foreach (var _event in events)
                if (_event.EventHandlerType != null)
                    AddType(_event.EventHandlerType);
        }      

        void AddTypeParameters(Type[] parameters)
        {
            foreach (var param in parameters)
                AddType(param);            
        }

        void AddTypeArguments(Type[] arguments)
        {
            foreach (var arg in arguments)
                AddType(arg);           
        }       

        void AddAssembly(TypeModel model)
        {
            var id = model.Type.Assembly.GetAssemblyId();
            // Add all needed assembly to the assemblies list
            if (!assemblies.ContainsKey(id))
                assemblies.Add(id, new AssemblyModel(model.Type.Assembly));
        }
    }
}
