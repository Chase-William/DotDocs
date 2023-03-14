using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models
{
    public class ProjectModel : Model
    {
        public string Name { get; set; }
        public string SDK { get; set; }
        public string TargetFramework { get; set; }
        public AssemblyModel Assembly { get; set; }
        public List<ProjectModel> Dependencies { get; set; } = new List<ProjectModel>();
    }
}
