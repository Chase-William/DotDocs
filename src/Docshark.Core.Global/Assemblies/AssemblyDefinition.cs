using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Docshark.Core.Global.Assemblies
{
    public class AssemblyDefinition : Definition
    {
        public override string PrimaryKey => Name;
        [JsonIgnore]
        public string Name { get; set; }
        public string ProjectForeignKey { get; set; }

        public static AssemblyDefinition From(Assembly assembly, string? name = null)
            => new()
            {
                Name = name ?? assembly.GetPrimaryKey()
            };
    }
}
