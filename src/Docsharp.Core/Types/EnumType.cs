using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace Docsharp.Core.Types
{
    public class EnumType : TypeMember<TypeInfo>
    {
        public override bool CanHaveInternalTypes => false;
        public override string Type => "Enum";

        public EnumType(TypeInfo member) : base(member)
        { }
    }
}
