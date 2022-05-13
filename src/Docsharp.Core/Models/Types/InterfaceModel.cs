using System.Reflection;

using Docsharp.Core.Models.Docs;
using Docsharp.Core.Models.Members;

namespace Docsharp.Core.Models.Types
{
    public class InterfaceModel : TypeMember<TypeInfo, Documentation>, INestable
    {
        public override bool CanHaveInternalTypes => true;
        public override string Type => "Interface";

        public string Namespace => Meta.Namespace;
        public string FullName => Meta.FullName;

        public PropertyModel[] Properties { get; set; }
        public FieldModel[] Fields { get; set; }
        public MethodModel[] Methods { get; set; }

        public InterfaceModel(TypeInfo member) : base(member)
        {
            INestable.Initialize(this, member);
        }
    }
}
