using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docsharp.Core.Metadata
{
    public enum MemberType
    {
        Type = 'T',
        Field = 'F',
        Property = 'P'
    }

    public class MemberDocs
    {
        public string FullName { get; set; }
        public MemberType Type { get; set; }
        public string Summary { get; set; }
        public List<Param> Params { get; set; } = new List<Param>();
        public string Returns { get; set; }
    }

    public class Param
    {
        public string Name { get; set; }
        public string Body { get; set; }
    }
}
