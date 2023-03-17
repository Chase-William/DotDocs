using DotDocs.Models.Util;
using Neo4j.Driver;
using Neo4jClient.Cypher;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
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
            //await ConnectRepoToRootProject();
            //await ConnectProjects();
            //await InsertAssemblies();
            //// await ConnectAssemblies();
            //using var session = GDC.GetSession();
            //await ConnectProjectToAssembly(session);
            //// await InsertTypes(session);
            //await session.CloseAsync();
        }

        async Task InsertTypes(IAsyncSession session)
        {
            // using var session = GDC.GetSession();
            // Used to track which assemblies have had their types already inserted
            var assemblyTracker = new List<AssemblyModel>();
            await Projects.First().InsertTypes(session, assemblyTracker);
            // await session.CloseAsync();
        }

        //async Task ConnectAssemblies()
        //{
        //    using var session = GDC.GetSession();
        //    await Projects.First().ConnectToAssembly(session);
        //    await session.CloseAsync();
        //}

        async Task ConnectProjectToAssembly(IAsyncSession session)
        {
            
            await Projects.First().ConnectToAssembly(session);
            
        }

        //async Task InsertAssemblies()
        //{
        //    var assemblies = new List<AssemblyModel>();
        //    Projects.First().GetProducedAssemblies(assemblies);

        //    using var session = GDC.GetSession();

        //    string cypher = new StringBuilder()
        //        .AppendLine("UNWIND $props AS map")
        //        .AppendLine("CREATE (p:Assembly { uid: apoc.create.uuid() })")
        //        .AppendLine("SET p += map")
        //        .AppendLine("RETURN p.uid, p.name")
        //        .ToString();

        //    try
        //    {
        //        var results = await session.RunAsync(cypher, new Dictionary<string, object>() { { "props", ParameterSerializer.ToDictionary(assemblies) } });
        //        var asmItems = await results.ToListAsync();

        //        // Get the ids from each project and feed it back into this application's projects
        //        foreach (var asm in assemblies)
        //        {
        //            var aItem = asmItems.Single(p => ((string)p["p.name"]) == asm.Name);
        //            asm.UID = (string)aItem["p.uid"];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine();
        //    }
        //}

        //async Task ConnectProjects()
        //{
        //    var root = Projects.First();
        //    using var session = GDC.GetSession();
        //    await root.ConnectProjects(session);
        //    await session.CloseAsync();
        //}

        //async Task ConnectRepoToRootProject()
        //{
        //    using var session = GDC.GetSession();

        //    string query = @"
        //        MATCH (p:Project { uid: $pid }), 
        //              (r:Repository { uid: $rid })
        //        CREATE (r)-[rel:HAS]->(p)";              

        //    try
        //    {
        //        // var test = Projects.First().UID;
        //        var result = await session.RunAsync(query, new
        //        {
        //            rid = UID,
        //            pid = Projects.First().UID
        //        });
        //        _ = result.ConsumeAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine();
        //    }
        //}

        async Task InsertRepo()
        {
            try
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