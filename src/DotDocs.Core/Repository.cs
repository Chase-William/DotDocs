using DotDocs.Core.Build;
using DotDocs.Core.Language;
using DotDocs.Core.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Collections.Immutable;
using System.Management.Automation;
using System.Reflection;

namespace DotDocs.Core
{
    public class Repository : IDisposable
    {
        /// <summary>
        /// Id for MongoDb records.
        /// </summary>
        public ObjectId Id { get; init; }

        [BsonIgnore]
        BuildInstance build;

        [BsonIgnore]
        CommentService comments;

        [BsonIgnore]
        Configuration config;

        public string Name { get; set; }
        /// <summary>
        /// The current commit hash of the repository.
        /// </summary>
        public string CommitHash { get; private set; }
        /// <summary>
        /// The url of the repository.
        /// </summary>
        public string Url { get; init; }

        [BsonIgnore]
        /// <summary>
        /// The directory of the repository.
        /// </summary>
        public string Dir { get; private set; }

        // public ImmutableList<SolutionFile> Solutions { get; private set; }

        [BsonIgnore]
        /// <summary>
        /// All project groups in the repository.
        /// </summary>
        public ImmutableArray<ProjectDocument> ProjectGraphs { get; private set; }

        [BsonIgnore]
        /// <summary>
        /// The select root project of a group to be documented.
        /// </summary>
        public ProjectDocument ActiveProject { get; private set; }

        [BsonIgnore]
        /// <summary>
        /// A collection containing only user defined models.
        /// </summary>
        public ImmutableArray<UserTypeModel> UserTypes { get; private set; }

        [BsonIgnore]
        public ImmutableDictionary<Type, TypeModel> AllTypes { get; private set; }

        [BsonIgnore]
        ImmutableArray<AssemblyModel<UserTypeModel>> userAssemblies;
        [BsonIgnore]
        ImmutableArray<AssemblyModel<TypeModel>> otherAssemblies;

        // public ImmutableArray<Models.AssemblyModel> UsedAssemblies { get; private set; }

        public Repository(string url, CommentService comments, Configuration config)
        {
            Url = url;
            this.comments = comments;
            this.config = config;

            Name = url.Split('/')[^1];

            if (Name.EndsWith(".git"))
                Name = Name[0..^4]; // Take until the last 4, remove .git
        }               

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
            powershell.AddScript($"git clone {Url}");
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
            build = new BuildInstance(ActiveProject)
                .Build()
                .MakeUserModels();                

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

        public Repository Prepare()
        {            
            userAssemblies = build.AllProjectBuildInstances
                .Select(proj => new AssemblyModel<UserTypeModel>(proj, config))
                .ToImmutableArray();

            UserTypes = userAssemblies
                .SelectMany(m => m.TypeModels)
                .Cast<UserTypeModel>()
                .ToImmutableArray();

            /*
             * Load all supporting types for all user created models & group their assemblies.
             */
            ProcessRequireTypes(UserTypes);

            /*
             * Aggregate all assemblies used for documentation later 
             */
            //UsedAssemblies = AllModels
            //    .DistinctBy(m => m.Value.Info.Assembly)
            //    .Select(m => new Models.AssemblyModel(m.Value.Info.Assembly))
            //    .ToImmutableArray();

            /*
             * Provide AssemblyModels with a reference to the list of their models from the build             
             */



            /*
             * Load all documentation for models from the database
             */
            comments.UpdateDocumentation(this, userAssemblies, otherAssemblies);

            return this;
        }

        public Repository Document()
        {
            var basePath = @"C:\Users\Chase Roth\Desktop";

            foreach (var userType in UserTypes)
            {
                userType.Document(basePath);
            }

            // Render documentation
            return this;
        }       

        /// <summary>
        /// Load all supporting types for all user created models.
        /// </summary>
        void ProcessRequireTypes(ImmutableArray<UserTypeModel> userTypes)
        {
            var allTypes = new Dictionary<Type, TypeModel>(userTypes
                    .Select(model => new KeyValuePair<Type, TypeModel>(model.Info, model)));

            var otherAssemblies = new Dictionary<Assembly, AssemblyModel<TypeModel>>();

            // Add model dependencies to the collection of all types            
            foreach (var model in userTypes)
                    model.Add(allTypes, otherAssemblies);

            AllTypes = allTypes.ToImmutableDictionary();

            this.otherAssemblies = otherAssemblies.Values.ToImmutableArray();
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
            if (Directory.Exists(Dir))
            {
                // Using powershell because Directory.Delete recursive cannot delete some files for some reason.
                // .git's objects/pack/*.dix and *.pack files.. their not locked, just dont have access to the path
                // This is my work around below:
                using PowerShell powershell = PowerShell.Create();
                powershell.AddScript($"rm -r -fo {Dir}");
                powershell.Invoke(); // Run powershell            
            }
        }
        
    }
}
