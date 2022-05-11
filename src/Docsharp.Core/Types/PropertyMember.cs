using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Docsharp.Core.Metadata;

namespace Docsharp.Core.Types
{
    public class PropertyMember : Member<PropertyInfo, Documentation>
    {
        public PropertyMember(PropertyInfo prop) : base(prop) { }

        public bool CanWrite => TypeInfo.CanWrite;
        public bool CanRead => TypeInfo.CanRead;
        public bool? IsSetPrivate => TypeInfo.SetMethod?.IsPrivate;
        public bool? IsGetPrivate => TypeInfo.GetMethod?.IsPrivate;
        public override string Type => TypeInfo.PropertyType.ToString();
    }
}
