using System.Collections.Generic;
using System.Reflection;

using Docsharp.Core.Models.Members;
using Docsharp.Core.Models.Docs;

namespace Docsharp.Core.Models.Types
{
    public class ClassModel : TypeMember<TypeInfo, Documentation>, INestable
    {
        public const string CLASS_TYPE_STRING = "class";
        public bool IsPublic => Meta.IsPublic;

        public override bool CanHaveInternalTypes => true;
        public override string Type => CLASS_TYPE_STRING;

        public PropertyModel[] Properties { get; set; }
        public FieldModel[] Fields { get; set; }
        public MethodModel[] Methods { get; set; }

        public ClassModel(TypeInfo member) : base(member)
        {
            INestable.Initialize(this, member);
        }
    }
}
