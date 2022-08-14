using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// in own library?

namespace Docshark.Core.Models
{
    public class TypeKey
    {
        public string ForeignKey { get; internal set; }
        public bool IsGeneric { get; internal set; }
    }
}
