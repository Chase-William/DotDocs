using System.Reflection;

using Docsharp.Core.Metadata;

namespace Docsharp.Core.Types
{
    public class DelegateType : TypeMember<TypeInfo, Documentation>, IFunctional
    {
        public override bool CanHaveInternalTypes => false;
        public override string Type => "Delegate";

        public new FunctionalDocumentation Docs => (FunctionalDocumentation)base.Docs;

        public string ReturnType { get; init; }
        public Parameter[] Parameters { get; init; }

        public DelegateType(TypeInfo member) : base(member)
        {
            var info = member.GetMethod("Invoke");
            ReturnType = info.ReturnType.ToString();
            Parameters = ((IFunctional)this).GetParameters(info);
        }        
    }
}
