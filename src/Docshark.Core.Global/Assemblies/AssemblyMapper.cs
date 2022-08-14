using Docshark.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Docshark.Core.Global.Assemblies
{
    public class AssemblyMapper : IMapper<AssemblyDefinition>
    {
        public const string ASSEMBLY_MAPPER_FILENAME = "assemblies.json";

        public IReadOnlyDictionary<string, AssemblyDefinition> MappedDefinitions => mappedDefinitions;

        Dictionary<string, AssemblyDefinition> mappedDefinitions = new();

        internal void AddAssembly(Assembly assembly)
        {
            var name = assembly.GetPrimaryKey();
            // Add the assembly to the map if not already accounted for
            if (!MappedDefinitions.ContainsKey(name))
                mappedDefinitions.Add(name, AssemblyDefinition.From(assembly, name));            
        }
    }
}
