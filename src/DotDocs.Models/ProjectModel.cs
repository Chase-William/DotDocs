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
    }
}
