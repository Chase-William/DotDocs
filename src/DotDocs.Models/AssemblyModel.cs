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
        public IEnumerable<TypeModel> Types { get; set; }
    }
}
