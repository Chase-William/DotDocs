using System.Reflection;

using Docsharp.Core.Models.Docs;
using Docsharp.Core.Models.Members;

namespace Docsharp.Core.Models.Types
{
    public class EnumModel : TypeMember<TypeInfo, Documentation>
    {
        public override bool CanHaveInternalTypes => false;
        public override string Type => "Enum";

        public FieldModel[] Values { get; set; }

        public EnumModel(TypeInfo member) : base(member)
        {
            // Omit first the element as it is provided by default by the compiler
            var fields = member.GetFields();
            Values = new FieldModel[fields.Length - 1];
            for (int i = 1; i < fields.Length; i++)
                Values[i - 1] = new FieldModel(fields[i]);
        }
    }
}
