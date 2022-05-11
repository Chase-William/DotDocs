using System.Reflection;

using Docsharp.Core.Metadata;

namespace Docsharp.Core.Types
{
    public class EnumType : TypeMember<TypeInfo, Documentation>
    {
        public override bool CanHaveInternalTypes => false;
        public override string Type => "Enum";

        public FieldMember[] Values { get; set; }

        public EnumType(TypeInfo member) : base(member)
        {
            // Omit first the element as it is provided by default by the compiler
            var fields = member.GetFields();
            Values = new FieldMember[fields.Length - 1];
            for (int i = 1; i < fields.Length; i++)            
                Values[i - 1] = new FieldMember(fields[i]);            
        }
    }
}
