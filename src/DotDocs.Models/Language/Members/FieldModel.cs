using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models.Language.Members
{
    internal class FieldModel : MemberModel
    {        
        public bool IsInitOnly { get; set; }
        public bool IsLiteral { get; set; }
        public bool IsNotSerialized { get; set; }
        public bool IsPInvokeImpl { get; set; }      
        public bool IsSecurityCritical { get; set; }
        public bool IsSecuritySafeCritical { get; set; }
        public bool IsSecurityTransparent { get; set; }        
        public string Name { get; set; }
    }
}
