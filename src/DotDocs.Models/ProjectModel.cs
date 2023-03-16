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
                await ConnectProjects();

            using var session = GDC.GetSession();
            // sid == sender id
            await session.RunAsync(@"
                MATCH (p1:Project { uid: $sid }), 
                UNWIND $projects AS proj
                MATCH (p:Project { uid: proj.uid })
                CREATE (p1)-[r:USES]->(p)
                ",
                new Dictionary<string, object>() 
                { 
                    { 
                        "props", ParameterSerializer.ToDictionary(Projects)                        
                    },
                    {
                        "uid", UID
                    }
                });
        }
    }
}
