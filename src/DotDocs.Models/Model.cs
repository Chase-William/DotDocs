using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace DotDocs.Models
{
    public abstract class Model
    {
        [JsonIgnore]
        public string UID { get; set; }
    }
}
