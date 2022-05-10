using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Docsharp.Core.Types
{
    public class FieldMember : Member<FieldInfo>
    {        
        public FieldMember(FieldInfo member) : base(member) { }

        public bool IsPublic => member.IsPublic;
        public bool IsReadonly => member.IsInitOnly;
        public bool IsConstant => member.IsLiteral;
        public bool IsStatic => member.IsStatic;
        public override string Type => member.FieldType.ToString();
    }
}
