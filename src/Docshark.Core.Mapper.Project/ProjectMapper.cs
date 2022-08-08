using Docshark.Core.Mapper.Project.Exceptions;
using Docshark.Core.Models.Codebase.Types;
using LoxSmoke.DocXml;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Docshark.Core.Models.Project
{
    public class ProjectMapper : IDisposable
    {
        const string PROJECT_NAME = "ProjectName";
        const string PROJECT_DIR = "ProjectDir";
        const string PROJECT_EXT = "ProjectExt";
        const string PROJECT_FILE_NAME = "ProjectFileName";
        const string PROJECT_PATH = "ProjectPath";
        const string TARGET_FILE_NAME = "TargetName";
        const string TARGET_PATH = "TargetPath";
        const string DOCUMENTATION_FILE = "DocumentationFile";

        [JsonIgnore]
        public string[] Assemblies { get; private set; }

        public LocalProject RootProject { get; private set; }          

        /**
         * msbuild ProjDepResolver.Runner.csproj -fl1 -fl2 -flp1:logfile=errors.log;errorsonly -flp2:logfile=details.log;details
         * dotnet msbuild ./ProjDepResolver.Runner/ProjDepResolver.Runner.csproj -fl1 -fl2 -flp1:logfile=errors.log;errorsonly -flp2:logfile=details.log;
         */

        public ProjectMapper() { }

        /// <summary>
        /// Saves both the <see cref="ProjectMapper"/> and the root project's trees.
        /// </summary>
        /// <param name="projectOutPath">Base location for project model tree.</param>
        /// <param name="metadataOutPath">Location for project tree.</param>
        public void Save(string projectOutPath, string metadataOutPath)
        {
            using var writer = new StreamWriter(Path.Combine(metadataOutPath, "project-tree.json"));           
            writer.Write(JsonSerializer.Serialize(this));
                        
            RootProject.Save(projectOutPath);
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

        //public bool ApplyRequiredConfiguration()
        //{
        //    var props = docFile.XPathSelectElement("//PropertyGroup");

        //    if (!generateDocumentationFile)
        //    {
        //        props.Add(new XElement("GenerateDocumentationFile")
        //        {
        //            Value = "True"
        //        });
        //        return true;
        //    }
        //    return false;
        //}

        //public void Save()
        //{
        //    using var stream = File.Open(projectFile, FileMode.Create);
        //    docFile.Save(stream);
        //}


        /*
         * 
         * Gather all .dlls too for each assembly
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

                var mainBuild = build.FindLastChild<Microsoft.Build.Logging.StructuredLogger.Project>();
                var target = mainBuild
                    .FindFirstChild<Target>(c => c.Name == "FindReferenceAssembliesForReferences");
                Assemblies = target.Children.Select(item => ((Item)((AddItem)item).FirstChild).Text).ToArray();

                // Get the root project
                var eval = build
                    .FindChild<TimedNode>("Evaluation");
                var projectEval = eval
                    .FindLastChild<ProjectEvaluation>(p => p.Name.Equals(projectName));

                RootProject = GetLocalProjectInfo(projectEval, new());
                // LocalProjects = GetLocalProjects(project);
                // OtherDependencies = GetOtherDependencies(project);

                //{ // Locate paths to all dependency assemblies
                //    var target = project
                //    .FindFirstChild<Target>(c => c.Name == "ResolveReferences");

                //    var refFolder = target
                //        .FindChild<Folder>("TargetOutputs");
                //    var references = refFolder.Children;
                //    depAsmPaths = references.Select(item => (item as Item).Text).ToArray();
                //}
                // depAsmPaths = new string[] { };
                //{
                //    var target = project
                //        .FindFirstChild<Target>(c => c.Name == "FindReferenceAssembliesForReferences");

                //    depAsmPaths = target.Children.Select(item => ((Item)((AddItem)item).FirstChild).Text).ToArray();
                //}

                //{ // Locate root assembly
                //    var target = project.FindChild<Target>("GetTargetPath");
                //    var outputs = target.FindChild<Folder>("TargetOutputs");

                //    var test = project.FindChildrenRecursive<Target>(t => t.Name.Equals("GetTargetPath"));

                //    // targetAsmPath = ((Item)outputs.Children.First()).ShortenedText;
                //}

                //{
                //    var target = project.FindChild<Target>("CoreCompile");

                //    var test = project.FindChildrenRecursive<Target>(t => t.Name.Equals("CoreCompile"));

                //    var outputs = target.FindChild<Folder>("Outputs");
                //    rootAsmPath = ((Item)outputs.Children.Single(item => ((Item)item).Text.Contains(".dll"))).Text;
                //}
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the local project's information and calls <see cref="GetLocalProjects(ProjectEvaluation)"/> to recursively aquire
        /// all sub projects too.
        /// </summary>
        /// <param name="projectEval">The project's build evaluation.</param>
        /// <returns>An instance to the <see cref="LocalProject"/>.</returns>
        LocalProject GetLocalProjectInfo(ProjectEvaluation projectEval, List<LocalProject> existingProjects)
        {            
            var props = projectEval.FindChild<Folder>("Properties");
            var properties = props.Children.Cast<NameValueNode>().Where(p =>
            {
                return p.Name switch
                {
                    PROJECT_DIR or 
                    PROJECT_EXT or 
                    PROJECT_FILE_NAME or 
                    PROJECT_PATH or 
                    TARGET_FILE_NAME or 
                    TARGET_PATH or
                    PROJECT_NAME or
                    DOCUMENTATION_FILE => true,
                    _ => false,
                };
            })                
            .ToDictionary(p => p.Name);

            string projDir = properties[PROJECT_DIR].Value;

            return new LocalProject
            {
                ProjectName = properties[PROJECT_NAME].Value,
                ProjectDirectory = projDir,
                ProjectFileExt = properties[PROJECT_EXT].Value,
                ProjectFileName = properties[PROJECT_FILE_NAME].Value,
                ProjectPath = properties[PROJECT_PATH].Value,
                AssemblyName = properties[TARGET_FILE_NAME].Value,
                AssemblyPath = properties[TARGET_PATH].Value,
                DocumentationPath = Path.Combine(projDir, properties[DOCUMENTATION_FILE].Value),
                LocalProjects = GetLocalProjects(projectEval, existingProjects)
            };
        }

        /// <summary>
        /// Gets all <see cref="LocalProject"/> dependencies of the given project evaluation.
        /// Operates in a depth-first-search mode.
        /// </summary>
        /// <param name="projectEval">Current project.</param>
        /// <param name="existingProjects">Contains a collection of all already loaded <see cref="LocalProject"/> to prevent duplicate instances.</param>
        /// <returns>All <see cref="LocalProject"/> dependencies of the current.</returns>
        LocalProject[] GetLocalProjects(ProjectEvaluation projectEval, List<LocalProject> existingProjects)
        {
            var items = projectEval.FindChild<Folder>("Items");
            var addItems = items.FindChild<AddItem>("ProjectReference");            
            if (addItems == null || addItems.Children.Count == 0)
                return Array.Empty<LocalProject>();

            IEnumerable<string> projectFileNames = addItems.Children.Cast<Item>().Select(p => p.Text[(p.Text.LastIndexOf('\\') + 1)..]);
            var eval = (TimedNode)projectEval.Parent;

            var projects = new List<LocalProject>();

            foreach (var projFileName in projectFileNames)
            {
                var projEval = eval.FindLastChild<ProjectEvaluation>(p => p.Name.Equals(projFileName));
                // Check to see if the project has already been loaded as a dependency elsewhere
                var existingProject = existingProjects.SingleOrDefault(p => p.ProjectFileName.Equals(projFileName));                
                if (existingProject != null) // Exist elsewhere so use existing instance
                    projects.Add(existingProject);
                else // Doesn't exist, create new
                {
                    var project = GetLocalProjectInfo(projEval, existingProjects);
                    // Update both existing project list and add to tree
                    existingProjects.Add(project);
                    projects.Add(project);
                }
            }

            //var targets = rootProject.FindChildrenRecursive<Target>(p => p.Name.Equals("ResolveProjectReferences") && p.Project == rootProject);
            //var msbuild = targets.First();
            //var build = msbuild.FindFirstChild<Microsoft.Build.Logging.StructuredLogger.Task>(p => p.Name.Equals("MSBuild"));
            //List<LocalProject> projs = new();
            //Microsoft.Build.Logging.StructuredLogger.Project current = build.FindChild<Microsoft.Build.Logging.StructuredLogger.Project>();

            //do // Get all local project dependencies
            //{
            //    projs.Add(GetLocalProject(current));
            //}
            //while ((current = msbuild.FindNextChild<Microsoft.Build.Logging.StructuredLogger.Project>(current)) != null);

            return projects.ToArray();

            //var task = target.FindChild<Microsoft.Build.Logging.StructuredLogger.Task>("MSBuild");
            //var projects = task.FindChildrenRecursive<Project>();
            //return projs.ToArray();
        }

        public void Load(string[] assemblies, Action<TypeMember<TypeInfo, TypeComments>> getTypeCallback)
        {            
            LoadRecursive(RootProject, assemblies, getTypeCallback);
        }

        void LoadRecursive(LocalProject project, string[] assemblies, Action<TypeMember<TypeInfo, TypeComments>> getTypeCallback)
        {
            project.Load(assemblies, getTypeCallback);
            // Visit the lowest level assembly and load it's info before loading higher level assemblies
            foreach (var proj in project.LocalProjects)
                LoadRecursive(proj, assemblies, getTypeCallback);            
        }

        /// <summary>
        /// Disposes all <see cref="LocalProject"/> within <see cref="RootProject"/> recursively.
        /// </summary>
        public void Dispose()
        {
            if (RootProject != null)
                DisposeNext(RootProject);            
            static void DisposeNext(LocalProject project)
            {
                foreach (var proj in project.LocalProjects)
                    DisposeNext(proj);
                project.Dispose();
            }
        }
    }
}
