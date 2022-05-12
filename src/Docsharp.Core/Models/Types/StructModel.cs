using System.Reflection;

using Docsharp.Core.Models.Members;
using Docsharp.Core.Models.Docs;

namespace Docsharp.Core.Models.Types
{
    public class StructModel : TypeMember<TypeInfo, Documentation>, IConstructable
    {
        public override bool CanHaveInternalTypes => true;

        public override string Type => "Struct";

        public PropertyModel[] Properties { get; set; }
        public FieldModel[] Fields { get; set; }
        public MethodModel[] Methods { get; set; }

        public StructModel(TypeInfo member) : base(member)
        {
            IConstructable.Initialize(this, member);
        }
    }
}
