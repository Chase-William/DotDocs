using System.Reflection;

using Docsharp.Core.Metadata;

namespace Docsharp.Core.Types
{
    public class StructType : TypeMember<TypeInfo, Documentation>, IConstructable
    {
        public override bool CanHaveInternalTypes => true;

        public override string Type => "Struct";

        public PropertyMember[] Properties { get; set; }
        public FieldMember[] Fields { get; set; }
        public MethodMember[] Methods { get; set; }

        public StructType(TypeInfo member) : base(member)
        {
            IConstructable.Initialize(this, member);
        }
    }
}
