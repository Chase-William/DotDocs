using System.Reflection;

using Docsharp.Core.Metadata;

namespace Docsharp.Core.Types
{
    public class FieldMember : Member<FieldInfo, Documentation>
    {        
        public FieldMember(FieldInfo member) : base(member) { }

        public bool IsPublic => TypeInfo.IsPublic;
        public bool IsReadonly => TypeInfo.IsInitOnly;
        public bool IsConstant => TypeInfo.IsLiteral;
        public bool IsStatic => TypeInfo.IsStatic;
        public override string Type => TypeInfo.FieldType.ToString();
    }
}
