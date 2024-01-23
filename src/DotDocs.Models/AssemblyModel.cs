using DotDocs.Models.Language;
using System.Collections.Immutable;
using System.Reflection;

namespace DotDocs.Models
{
    public class AssemblyModel : Model
    {
        public string Name { get; set; }
        public List<TypeModel> Types { get; set; } = new();

        public AssemblyModel(
            Assembly assembly,
            Dictionary<string, AssemblyModel> assemblies,
            Dictionary<string, ITypeable> types)
        {            
            Name = assembly.GetName().Name;            
            assemblies.Add(assembly.FullName, this);

            List<TypeModel> models = new();
            foreach (var type in assembly.GetExportedTypes())
            {
                models.Add(new TypeModel(type, assemblies.ToImmutableDictionary(), types));//.Apply(type));
            }
            Types = models;
        }
    }
}
