using DotDocs.Models.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models
{
    public class AssemblyModel : Model
    {
        public string Name { get; set; }
        public List<TypeModel> Types { get; set; } = new();
    }
}
