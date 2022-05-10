using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Docsharp.Core.Types
{
    public class InterfaceType : TypeMember<TypeInfo>
    {
        public override bool CanHaveInternalTypes => true;
        public override string Type => "Interface";

        public InterfaceType(TypeInfo member) : base(member)
        { }

        public string Namespace => member.Namespace;
        public string FullName => member.FullName;
    }
}
