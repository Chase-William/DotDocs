using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Core.Models.Mongo
{
    public record class AssemblyModel
    {
        public string Name { get; set; }
        public Version Version { get; set; }
    }
}
