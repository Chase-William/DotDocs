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
            await InsertRepo();
            await InsertProjects();
        }

        async Task InsertRepo()
        {
            var session = GDC.driver.AsyncSession(o => o.WithDatabase("neo4j"));

            string query = @"
                CREATE (r:Repository {
                    id: apoc.create.uuid(),
                    name: $name,
                    url: $url,
                    commit: $commit
                })
                RETURN r.id";

            try
            {
                var result = await session.RunAsync(query, new
                {
                    name = Name,
                    url = Url,
                    commit = Commit
                });
                var record = await result.SingleAsync();
                Id = (string)record["r.id"];
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
            finally
            {
                session.Dispose();
            }           
        }

        async Task InsertProjects()
        {
            var projects = new List<ProjectModel>();
            Projects.First().Flat(projects);

            IAsyncSession? session = null;
            
            // https://github.com/bytefish/Neo4JSample

            string cypher = new StringBuilder()
                .AppendLine("UNWIND $props AS map")
                .AppendLine("CREATE (p:Project { id: apoc.create.uuid() })")
                .AppendLine("SET p += map")
                .AppendLine("RETURN p.id, p.name")
                .ToString();

            // https://gist.github.com/nandosola/ebe2ced123e05a79e238edd6ec81fee5

            try
            {
                session = GDC.driver.AsyncSession(o => o.WithDatabase("neo4j"));               
                    
                var results = await session.RunAsync(cypher, new Dictionary<string, object>() { { "props", ParameterSerializer.ToDictionary(projects) } });
                var projItems = await results.ToListAsync();

                // Get the ids from each project and feed it back into this application's projects
                foreach (var proj in projects)
                {
                    var pItem = projItems.Single(p => ((string)p["p.name"]) == proj.Name);
                    proj.Id = (string)pItem["p.id"];
                }

                //foreach (var item in projItems)
                //{

                //    var proj = projects.Single(p => p.Name == item["p.name"] as string);
                //    proj.Id = 
                //}

                //var id = r[0]["p.id"];
                //var name = r[1]["p.name"];
                Console.WriteLine();
                //string query = @"
                //    CREATE (r:Repository { 
                //        id: apoc.create.uuid(),
                //        name: $name,
                //        url: $url,
                //        commit: $commit
                //    })";

                //await session.RunAsync(query, new
                //{
                //    name = Name,
                //    url = Url,
                //    commit = Commit
                //});
                // var items = await result.ToListAsync();
                Console.WriteLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine();
            }
            finally
            {
                session.Dispose();
            }

            //foreach (var proj in projects)
            //{
            //    queue.Add(Task.Run(Test));

            //    async Task Test()
            //    {
            //        // Insert project
            //        IResultCursor cursor = await session.RunAsync(query, new
            //        {
            //            name = proj.Name
            //        });
            //    }

            //}

            //try
            //{                                
            //    await Task.WhenAll(queue);
            //    Console.WriteLine();
            //}
            //finally
            //{
            //    session.Dispose();
            //}            
                      


            //StringBuilder builder = new StringBuilder();

            //int count = 0;
            //foreach (var proj in projects)
            //{
            //    if (count == 0)
            //        builder.Append(@"
            //            CREATE (p0:Project { 
            //                name: $name
            //            })");
            //    else
            ////        builder.Append(@$" ,(p{count} {{ name: {} }}");
            ////    count++;
            ////}

            //string query = @"
            //    CREATE (p:Project { 
            //        name: $name
            //    })
            //    RETURN p";

            //try
            //{
            //    // Insert project
            //    IResultCursor cursor = await session.RunAsync(query, new
            //    {
            //        name = Name
            //    });

            //    // var id = await cursor.SingleAsync(r => r["p"].As<INode>().ElementId);
            //    //Id = id;
            //    //var model = await cursor.SingleAsync(r => r.As<ProjectModel>());
            //    // Id = model.Id;
            //    // var test = cursor.Consume();

            //    //foreach (var proj in Projects)
            //    //{
            //    //    Console.WriteLine();
            //    //    var r = await session.RunAsync(@"
            //    //        MATCH (t:Project { id: $tid }), 
            //    //              (o:Project { id: $oid }) 
            //    //        CREATE (t)-[r:USES]->(o)",
            //    //        new
            //    //        {
            //    //            tid = Id,
            //    //            oid = proj.Id
            //    //        });
            //    //}

            //    // var currentProjectId = records.First().As<ProjectModel>().Id;

            //    // Insert Relationships for projects if needed
            //    //if (Projects.Count > 0)
            //    //{

            //    //    foreach (var proj in Projects)
            //    //    {
            //    //        session.Run(@"
            //    //            MATCH (this:Project { id: $tid }), (other:Project { id: $oid }) 
            //    //            CREATE (this)-[r:USES]->(other)",
            //    //            new
            //    //            {
            //    //                tid = currentProjectId,
            //    //                oid = proj.Id
            //    //            });
            //    //    }                    
            //    //}                
            //}
            //finally
            //{
            //    session.Dispose();
            //}

            //return await session.ExecuteWriteAsync(async tx => {
            //    var result = await tx.RunAsync(query, new { name = Name });
            //    return await result.SingleAsync();
            //});           
        }
    }
}