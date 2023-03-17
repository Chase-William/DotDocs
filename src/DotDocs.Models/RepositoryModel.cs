using DotDocs.Models.Util;
using Neo4j.Driver;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        public async void Run()
        {
            await InsertProjects();
            await InsertRepo();            
            await ConnectRepoToRootProject();
            await ConnectProjects();
            await InsertAssemblies();
            await ConnectProjectToAssembly();
        }

        async Task ConnectProjectToAssembly()
        {
            using var session = GDC.GetSession();
            await Projects.First().ConnectToAssembly(session);
            await session.CloseAsync();
        }

        async Task InsertAssemblies()
        {
            var assemblies = new List<AssemblyModel>();
            Projects.First().GetProducedAssemblies(assemblies);

            using var session = GDC.GetSession();

            string cypher = new StringBuilder()
                .AppendLine("UNWIND $props AS map")
                .AppendLine("CREATE (p:Assembly { uid: apoc.create.uuid() })")
                .AppendLine("SET p += map")
                .AppendLine("RETURN p.uid, p.name")
                .ToString();

            try
            {
                var results = await session.RunAsync(cypher, new Dictionary<string, object>() { { "props", ParameterSerializer.ToDictionary(assemblies) } });
                var asmItems = await results.ToListAsync();

                // Get the ids from each project and feed it back into this application's projects
                foreach (var asm in assemblies)
                {
                    var aItem = asmItems.Single(p => ((string)p["p.name"]) == asm.Name);
                    asm.UID = (string)aItem["p.uid"];
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
            using var session = GDC.GetSession();
            await root.ConnectProjects(session);
            await session.CloseAsync();
        }

        async Task ConnectRepoToRootProject()
        {
            using var session = GDC.GetSession();

            string query = @"
                MATCH (p:Project { uid: $pid }), 
                      (r:Repository { uid: $rid })
                CREATE (r)-[rel:HAS]->(p)";              

            try
            {
                // var test = Projects.First().UID;
                var result = await session.RunAsync(query, new
                {
                    rid = UID,
                    pid = Projects.First().UID
                });
                _ = result.ConsumeAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
        }

        async Task InsertRepo()
        {
            using var session = GDC.GetSession();

            string query = @"
                CREATE (r:Repository {
                    uid: apoc.create.uuid(),
                    name: $name,
                    url: $url,
                    commit: $commit
                })
                RETURN r.uid";

            try
            {
                var result = await session.RunAsync(query, new
                {
                    name = Name,
                    url = Url,
                    commit = Commit
                });
                var record = await result.SingleAsync();
                UID = (string)record["r.uid"];               
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

            using var session = GDC.GetSession();

            // https://github.com/bytefish/Neo4JSample

            string cypher = new StringBuilder()
                .AppendLine("UNWIND $props AS map")
                .AppendLine("CREATE (p:Project { uid: apoc.create.uuid() })")
                .AppendLine("SET p += map")
                .AppendLine("RETURN p.uid, p.name")
                .ToString();

            // https://gist.github.com/nandosola/ebe2ced123e05a79e238edd6ec81fee5

            try
            {                                             
                var results = await session.RunAsync(cypher, new Dictionary<string, object>() { { "props", ParameterSerializer.ToDictionary(projects) } });
                var projItems = await results.ToListAsync();

                // Get the ids from each project and feed it back into this application's projects
                foreach (var proj in projects)
                {
                    var pItem = projItems.Single(p => ((string)p["p.name"]) == proj.Name);
                    proj.UID = (string)pItem["p.uid"];
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine();
            }
            finally
            {
                await session.CloseAsync();
            }
        }
    }
}