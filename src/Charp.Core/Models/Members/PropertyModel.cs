using System.Reflection;

using LoxSmoke.DocXml;

namespace Charp.Core.Models.Members
{
    public class PropertyModel : Member<PropertyInfo, CommonComments>
    {
        public PropertyModel(PropertyInfo prop) : base(prop) { }

        public bool CanWrite => Meta.CanWrite;
        public bool CanRead => Meta.CanRead;
        public bool? IsSetPrivate => Meta.SetMethod?.IsPrivate;
        public bool? IsGetPrivate => Meta.GetMethod?.IsPrivate;
        public override string Type => Meta.PropertyType.ToString();
    }
}
