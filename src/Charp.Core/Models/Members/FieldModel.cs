using System.Reflection;

using LoxSmoke.DocXml;

namespace Charp.Core.Models.Members
{
    public class FieldModel : Member<FieldInfo, CommonComments>
    {        
        public bool IsPublic => Meta.IsPublic;
        public bool IsReadonly => Meta.IsInitOnly;
        public bool IsConstant => Meta.IsLiteral;
        public bool IsStatic => Meta.IsStatic;
        public override string Type => Meta.FieldType.ToString();

        public FieldModel(FieldInfo member) : base(member) { }
    }
}
