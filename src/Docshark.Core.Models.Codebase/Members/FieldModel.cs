using System.Reflection;
using Docshark.Core.Models.Codebase;
using LoxSmoke.DocXml;

namespace Docshark.Core.Models.Codebase.Members
{
    public class FieldModel : Model<FieldInfo, CommonComments>, IAccessible
    {                    
        public bool IsReadonly => Meta.IsInitOnly;
        public bool IsConstant => Meta.IsLiteral;
        public bool IsStatic => Meta.IsStatic;
        public override string Type => Meta.FieldType.ToString();

        #region IAccessible
        public bool IsPublic => Meta.IsPublic;
        public bool IsPrivate => Meta.IsPrivate;
        public bool IsProtected => Meta.IsFamily || Meta.IsFamilyOrAssembly;
        public bool IsInternal => Meta.IsAssembly || Meta.IsFamilyOrAssembly;
        #endregion

        public FieldModel(FieldInfo member) : base(member) { }
    }
}
