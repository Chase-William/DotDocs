using DotDocs.Core.Comments;
using LoxSmoke.DocXml;
using System.Reflection;

namespace DotDocs.Core.Language.Members
{
    public class FieldModel : MemberModel<FieldInfo, CommonCommentsModel<CommonComments>>
    {
        public bool IsReadonly => Info.IsInitOnly;
        public bool IsLiteral => Info.IsLiteral;
        public bool IsStatic => Info.IsStatic;

        #region Accessiblity
        public bool IsPublic => Info.IsPublic;
        public bool IsPrivate => Info.IsPrivate;
        public bool IsProtected => Info.IsFamily || Info.IsFamilyOrAssembly;
        public bool IsInternal => Info.IsAssembly || Info.IsFamilyOrAssembly;
        #endregion

        public object? RawConstantValue => IsLiteral ? Info.GetRawConstantValue() : null;

        public FieldModel(FieldInfo member) : base(member) { }

        public FieldModel(FieldInfo member, Type underlyingType) : base(member) { }
    }
}
