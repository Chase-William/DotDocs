using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace Docsharp.Core.Types
{
    public class StructType : TypeMember<TypeInfo>
    {
        public override bool CanHaveInternalTypes => true;

        public override string Type => "Struct";

        public StructType(TypeInfo member) : base(member)
        { }        
    }
}
