using DotDocs.Models.Language;
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
