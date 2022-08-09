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
        public string AssemblyName { get; set; }
        public string ProjectForeignKey { get; set; }

        public static AssemblyDefinition From(Assembly assembly, string? name = null)
            => new()
            {
                AssemblyName = name ?? assembly.GetPrimaryKey()
            };

        public override string GetPrimaryKey() 
            => AssemblyName;

        internal static string GetPrimaryKeyMemberName()
            => nameof(AssemblyName);
    }
}
