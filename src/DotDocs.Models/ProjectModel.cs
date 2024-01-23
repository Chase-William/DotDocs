using System.Reflection;

namespace DotDocs.Models
{
    public class ProjectModel : IDisposable
    {        
        public string Name { get; set; }
        
        public string SDK { get; set; } = string.Empty;
        
        public string TargetFramework { get; set; } = string.Empty;
        
        public Assembly Assembly { get; set; }
        
        public List<ProjectModel> Projects { get; set; } = new();

        IDisposable _mlc;

        public ProjectModel(string name, Assembly assembly, IDisposable mlc)
        {
            Name = name;
            Assembly = assembly;
            _mlc = mlc;
        }

        public void Dispose()
        {
            _mlc.Dispose();
        }
    }
}
