using LoxSmoke.DocXml;
using System.Reflection;

namespace Docsharp.Core.Models.Types
{
    public class DelegateModel : TypeMember<TypeInfo, TypeComments>, IFunctional
    {
        public const string DELEGATE_TYPE_STRING = "delegate";
        public override bool CanHaveInternalTypes => false;
        public override string Type => DELEGATE_TYPE_STRING;

        // public new FunctionalDocumentation Docs => (FunctionalDocumentation)base.Docs;

        public string ReturnType { get; init; }
        public Parameter[] Parameters { get; init; }

        public DelegateModel(TypeInfo member) : base(member)
        {
            var info = member.GetMethod("Invoke");
            ReturnType = info.ReturnType.ToString();
            Parameters = ((IFunctional)this).GetParameters(info);
        }
    }
}
