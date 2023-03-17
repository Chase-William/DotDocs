using Neo4j.Driver;
using Neo4jClient.Cypher;
using GDC = DotDocs.Models.GraphDatabaseConnection;

namespace DotDocs.Models
{
    public class RepositoryModel : Model
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Commit { get; set; }
        public DateTime Added { get; set; }
        public List<ProjectModel> Projects { get; set; } = new();

        public async Task Run()
        {
            await InsertProjects();
            await InsertRepo();            
            await ConnectRepoToRootProject();
            await ConnectProjects();
            await InsertAssemblies();
            await ConnectAssemblies();
            await InsertTypes();
            // await ConnectTypes();
        }

        //async Task ConnectTypes()
        //{

        //}

        async Task InsertTypes()
        {
            // Used to track which assemblies have had their types already inserted
            var assemblyTracker = new List<AssemblyModel>();
            await Projects.First().InsertTypes(assemblyTracker);
        }

        async Task ConnectAssemblies()
        {
            await Projects.First().ConnectToAssembly();
        }

        async Task InsertAssemblies()
        {
            var assemblies = new List<AssemblyModel>();
            Projects.First().GetProducedAssemblies(assemblies);

            var query = GDC.Client.Cypher
                .Unwind("$assemblies", "props")
                .WithParam("assemblies", assemblies)
                .Create("(a:Assembly { uid: apoc.create.uuid() })")
                .Set("a += props")
                .Return(a =>
                new {
                    UID = Return.As<string>("a.uid"),
                    Name = Return.As<string>("a.name")
                });

            try
            {
                var col = query.ResultsAsync.Result;

                // Get the ids from each project and feed it back into this application's projects
                foreach (var asm in assemblies)
                {
                    var aItem = col.Single(p => p.Name == asm.Name);
                    asm.UID = aItem.UID;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
        }

        async Task ConnectProjects()
        {
            var root = Projects.First();
            await root.ConnectProjects();
        }

        async Task ConnectRepoToRootProject()
        {
            var test = new
            {
                puid = Projects.First().UID,
                ruid = UID
            };
            var query = GDC.Client.Cypher
                .Match("(p:Project { uid: $puid }), (r:Repository { uid: $ruid })")
                .WithParams(test)
                .Create("(r)-[rel:HAS]->(p)");

            try
            {
                query.ExecuteWithoutResultsAsync().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
        }

        async Task InsertRepo()
        {
            var query = GDC.Client.Cypher
                .Create("(r:Repository { uid: apoc.create.uuid(), name: $name, url: $url, commit: $commit })")
                .WithParams(new
                {
                    name = Name,
                    url = Url,
                    commit = Commit
                })
                .Return((r) => new
                {
                    UID = Return.As<string>("r.uid")
                });

            try
            {
                var col = query.ResultsAsync.Result;
                UID = col.First().UID;
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
        }

        async Task InsertProjects()
        {
            var projects = new List<ProjectModel>();
            Projects.First().Flat(projects);

            var query = GDC.Client.Cypher
                .Unwind("$projects", "props")
                .WithParam("projects", projects)
                .Create("(p:Project { uid: apoc.create.uuid() })")
                .Set("p += props")
                .Return((p) => new
                {
                    UID = Return.As<string>("p.uid"),
                    Name = Return.As<string>("p.name")
                });

            try
            {
                var col = query.ResultsAsync.Result;
                foreach (var proj in projects)
                {
                    var item = col.Single(p => p.Name == proj.Name);
                    proj.UID = item.UID;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
        }
    }
}