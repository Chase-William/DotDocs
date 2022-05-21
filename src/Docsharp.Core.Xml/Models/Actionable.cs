using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docsharp.Core.Xml.Models
{
    public class Actionable : Entity
    {
        public List<Parameter> Params { get; set; } = new();
        public string Returns { get; set; }        
    }
}
