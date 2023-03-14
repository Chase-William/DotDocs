using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models.Language.Members
{
    public class MemberModel : Model
    {
        public bool IsAssembly { get; set; }
        public bool IsFamily { get; set; }
        public bool IsFamilyAndAssembly { get; set; }
        public bool IsFamilyOrAssembly { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsPublic { get; set; }
        public bool IsSpecialName { get; set; }
        public bool IsStatic { get; set; }
        public string Name { get; set; }
    }
}
