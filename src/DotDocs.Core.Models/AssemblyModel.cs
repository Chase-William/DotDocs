using DotDocs.Core.Models.Language;
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
        public string Id
            => assemblyId ??= Assembly.GetAssemblyId();        

        /// <summary>
        /// The underlying assembly instance from the runtime.
        /// </summary>
        [JsonIgnore]
        public Assembly Assembly { get; init; }

        /// <summary>
        /// A reference to the local project that creates this assembly if it exists in the context.
        /// </summary>
        [JsonIgnore]
        public LocalProjectModel? LocalProject { get; set; }

        /// <summary>
        /// Contains all the types defined specifically in this assembly.
        /// </summary>
        [JsonIgnore]
        public List<TypeModel> Types { get; set; } = new();

        public AssemblyModel(Assembly assembly)
        {
            Assembly = assembly;
        }
    }
}
