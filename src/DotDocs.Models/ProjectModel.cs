namespace DotDocs.Models
{
    public class ProjectModel : Model
    {        
        public string Name { get; set; }
        
        public string SDK { get; set; } = string.Empty;
        
        public string TargetFramework { get; set; } = string.Empty;
        
        public AssemblyModel Assembly { get; set; }
        
        public List<ProjectModel> Projects { get; set; } = new();        
    }
}
