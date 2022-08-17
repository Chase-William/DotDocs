using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DotDocs.Core.Models
{
    public class AssemblyModel
    {
        public string? Name => Assembly.GetName().Name;

        public string FullName => Assembly.FullName;        

        string assemblyId;
        public string AssemblyId
            => assemblyId ??= Assembly.GetAssemblyId();

        [JsonIgnore]
        public Assembly Assembly { get; init; }

        public AssemblyModel(Assembly assembly)
        {
            Assembly = assembly;
        }
    }
}
