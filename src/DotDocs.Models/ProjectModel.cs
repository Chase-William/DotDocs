using Neo4j.Driver;
using System.Text.Json.Serialization;
using GDC = DotDocs.Models.GraphDatabaseConnection;
using Newtonsoft.Json;

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
    }
}
