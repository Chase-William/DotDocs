using System.Collections.Generic;
using System.Reflection;

using Docsharp.Core.Metadata;

namespace Docsharp.Core.Types
{
    public class ClassType : TypeMember<TypeInfo, Documentation>, IConstructable
    {        
        public string Namespace => TypeInfo.Namespace;
        public string FullName => TypeInfo.FullName;
        public bool IsPublic => TypeInfo.IsPublic;

        public override bool CanHaveInternalTypes => true;
        public override string Type => "Class";

        public PropertyMember[] Properties { get; set; }
        public FieldMember[] Fields { get; set; }
        public MethodMember[] Methods { get; set; }

        public ClassType(TypeInfo member) : base(member)
        {
            IConstructable.Initialize(this, member);
        }
    }
}
