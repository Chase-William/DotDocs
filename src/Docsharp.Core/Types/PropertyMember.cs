using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Docsharp.Core.Types
{
    public class PropertyMember : Member<PropertyInfo>
    {
        public PropertyMember(PropertyInfo prop) : base(prop) { }

        public bool CanWrite => member.CanWrite;
        public bool CanRead => member.CanRead;
        public bool? IsSetPrivate => member.SetMethod?.IsPrivate;
        public bool? IsGetPrivate => member.GetMethod?.IsPrivate;
        public override string Type => member.PropertyType.ToString();
    }
}
