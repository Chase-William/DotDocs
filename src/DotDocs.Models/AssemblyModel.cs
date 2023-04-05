using DotDocs.Models.Language;
using DotDocs.Models.Util;
using Neo4j.Driver;
using Neo4jClient.Cypher;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDC = DotDocs.Models.GraphDatabaseConnection;

namespace DotDocs.Models
{
    public class AssemblyModel : Model
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonIgnore]
        public List<TypeModel> Types { get; set; } = new();
        [JsonIgnore]
        public List<AssemblyModel> Assemblies { get; set; } = new();

        internal void InsertTypes(List<AssemblyModel> assemblies)
        {
            // Avoid adding an assembly's types again...
            if (assemblies.Contains(this))
                return;

            var query = GDC.Client.Cypher
                .Match("(a:Assembly { uid: $auid })")
                .WithParam("auid", UID)
                .Unwind("$types", "props")
                .WithParam("types", Types)
                .Create("(t:Type { uid: apoc.create.uuid() })<-[rel:HAS]-(a)")
                .Set("t += props")
                .Return(t => new
                {
                    UID = Return.As<string>("t.uid"),
                    FullName = Return.As<string>("t.fullName")
                });

            try
            {
                var col = query.ResultsAsync.Result;

                // Get the ids from each project and feed it back into this application's projects
                foreach (var type in Types)
                {
                    var tItem = col.SingleOrDefault(t => t.FullName == type.FullName);
                    if (tItem == null)
                    {
                        return;
                    }
                    type.UID = tItem.UID;
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
            finally
            {
                assemblies.Add(this);
            }
        }

        //internal async Task ConnectProjects()
        //{
        //    foreach (var item in Projects)
        //        await item.ConnectProjects();

        //    // Prevent query from operating on an empty list
        //    if (Projects.Count == 0)
        //        return;

        //    foreach (var proj in Projects)
        //    {
        //        using var session = GDC.GetSession();

        //        try
        //        {
        //            // sid == sender id, rid == receiving id
        //            await session.RunAsync(@"
        //                MATCH (p1:Project { uid: $sid }),
        //                      (p2:Project { uid: $rid })
        //                CREATE (p1)-[r:USES]->(p2)
        //            ",
        //            new
        //            {
        //                sid = UID,
        //                rid = proj.UID
        //            });
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine();
        //        }
        //    }
        //}
    }
}
