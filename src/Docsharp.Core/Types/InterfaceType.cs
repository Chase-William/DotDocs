using System.Reflection;

using Docsharp.Core.Metadata;

namespace Docsharp.Core.Types
{
    public class InterfaceType : TypeMember<TypeInfo, Documentation>, IConstructable
    {
        public override bool CanHaveInternalTypes => true;
        public override string Type => "Interface";        

        public string Namespace => TypeInfo.Namespace;
        public string FullName => TypeInfo.FullName;

        public PropertyMember[] Properties { get; set; }
        public FieldMember[] Fields { get; set; }
        public MethodMember[] Methods { get; set; }

        public InterfaceType(TypeInfo member) : base(member)
        {
            IConstructable.Initialize(this, member);
        }
    }
}
