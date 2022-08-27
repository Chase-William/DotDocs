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
    /// <summary>
    /// A class that represents an assembly as a model.
    /// </summary>
    public class AssemblyModel
    {
        /// <summary>
        /// <inheritdoc cref="AssemblyName.Name"/>
        /// </summary>
        public string? Name => Assembly.GetName().Name;
        /// <summary>
        /// <inheritdoc cref="Assembly.FullName"/>
        /// </summary>
        public string FullName => Assembly.FullName;        
        /// <summary>
        /// The underlying field for <see cref="Id"/>.
        /// </summary>
        string assemblyId;
        /// <summary>
        /// The id for this assembly.
        /// </summary>
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
        /// <summary>
        /// Creates a new instance of the <see cref="AssemblyModel"/> class.
        /// </summary>
        /// <param name="assembly">Assembly this model represents.</param>
        public AssemblyModel(Assembly assembly)
        {
            Assembly = assembly;
        }
    }
}
