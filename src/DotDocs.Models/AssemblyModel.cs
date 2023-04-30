using DotDocs.Models.Language;
using DotDocs.Models.Util;
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
    }
}
