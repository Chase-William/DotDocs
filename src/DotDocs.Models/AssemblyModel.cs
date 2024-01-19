using DotDocs.Models.Language;

namespace DotDocs.Models
{
    public class AssemblyModel : Model
    {
        public string Name { get; set; }
        public List<TypeModel> Types { get; set; } = new();
        public List<AssemblyModel> Assemblies { get; set; } = new();    
    }
}
