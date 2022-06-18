using System.Reflection;

using LoxSmoke.DocXml;

namespace Charp.Core.Models.Members
{
    public class FieldModel : Model<FieldInfo, CommonComments>, IMemberable
    {                    
        public bool IsReadonly => Meta.IsInitOnly;
        public bool IsConstant => Meta.IsLiteral;
        public bool IsStatic => Meta.IsStatic;
        public override string Type => Meta.FieldType.ToString();

        #region IMemeberable
        public bool IsPublic => Meta.IsPublic;
        public bool IsProtected => Meta.IsFamily || Meta.IsFamilyOrAssembly;
        public bool IsInternal => Meta.IsAssembly || Meta.IsFamilyOrAssembly;
        #endregion

        public FieldModel(FieldInfo member) : base(member) { }
    }
}
