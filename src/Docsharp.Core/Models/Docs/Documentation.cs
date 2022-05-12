using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docsharp.Core.Models.Docs
{
    public class Documentation
    {
        public string FullName { get; set; }
        public MemberType Type { get; set; }
        public string Summary { get; set; }
    }
}
