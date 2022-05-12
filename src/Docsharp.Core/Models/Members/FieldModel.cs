using System.Reflection;

using Docsharp.Core.Models.Docs;

namespace Docsharp.Core.Models.Members
{
    public class FieldModel : Member<FieldInfo, Documentation>
    {
        public FieldModel(FieldInfo member) : base(member) { }

        public bool IsPublic => Meta.IsPublic;
        public bool IsReadonly => Meta.IsInitOnly;
        public bool IsConstant => Meta.IsLiteral;
        public bool IsStatic => Meta.IsStatic;
        public override string Type => Meta.FieldType.ToString();
    }
}
