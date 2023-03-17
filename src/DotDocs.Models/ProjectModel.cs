using Neo4j.Driver;
using System.Text.Json.Serialization;
using GDC = DotDocs.Models.GraphDatabaseConnection;
using Newtonsoft.Json;
using DotDocs.Models.Util;

namespace DotDocs.Models
{
    public class ProjectModel : Model
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("sdk")]
        public string SDK { get; set; } = string.Empty;
        [JsonProperty("targetFramework")]
        public string TargetFramework { get; set; } = string.Empty;
        [Newtonsoft.Json.JsonIgnore]
        public AssemblyModel Assembly { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public List<ProjectModel> Projects { get; set; } = new();       

        internal void Flat(List<ProjectModel> projects)
        {
            foreach (var proj in Projects)
                proj.Flat(projects);
            projects.Add(this);
        }

        internal async Task ConnectProjects()
        {
            foreach (var item in Projects)            
                await item.ConnectProjects();

            // Prevent query from operating on an empty list
            if (Projects.Count == 0)
                return;

            foreach (var proj in Projects)
            {
                using var session = GDC.GetSession();

                try
                {
                    // sid == sender id, rid == receiving id
                    await session.RunAsync(@"
                        MATCH (p1:Project { uid: $sid }),
                              (p2:Project { uid: $rid })
                        CREATE (p1)-[r:USES]->(p2)
                    ",
                    new
                    {
                        sid = UID,
                        rid = proj.UID
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                }
            }                       
        }
    }
}
