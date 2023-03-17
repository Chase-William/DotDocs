using DotDocs.Models.Language;
using DotDocs.Models.Util;
using Neo4j.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        internal async Task InsertTypes(IAsyncSession session, List<AssemblyModel> assemblies)
        {
            if (assemblies.Contains(this))
                return;

            string cypher = new StringBuilder()
                .AppendLine("UNWIND $props AS map")
                .AppendLine("CREATE (p:Type { uid: apoc.create.uuid() })")
                .AppendLine("SET p += map")
                .AppendLine("RETURN p.uid, p.name")
                .ToString();

            try
            {
                var results = await session.RunAsync(cypher, new Dictionary<string, object>() { { "props", ParameterSerializer.ToDictionary(Types) } });
                var typeItems = await results.ToListAsync();

                // Get the ids from each project and feed it back into this application's projects
                foreach (var type in Types)
                {
                    var tItem = typeItems.Single(p => ((string)p["p.name"]) == type.Name);
                    type.UID = (string)tItem["p.uid"];
                }
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
