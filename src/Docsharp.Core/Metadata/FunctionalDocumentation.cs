using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docsharp.Core.Metadata
{
    public class FunctionalDocumentation : Documentation
    {
        public List<ParamDocumentation> Params { get; set; } = new();
        public string Returns { get; set; }
    }
}
