using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace Docsharp.Core.Types
{
    public class DelegateType : TypeMember<TypeInfo>
    {
        public override bool CanHaveInternalTypes => false;
        public override string Type => "Delegate";

        public DelegateType(TypeInfo member) : base(member)
        { }        
    }
}
