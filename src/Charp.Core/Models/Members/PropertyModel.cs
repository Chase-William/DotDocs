using System.Reflection;

using LoxSmoke.DocXml;

namespace Charp.Core.Models.Members
{
    public class PropertyModel : Member<PropertyInfo, CommonComments>
    {        
        public bool CanSet => Meta.CanWrite;
        public bool CanGet => Meta.CanRead;
        public bool IsSetPrivate => Meta.SetMethod?.IsPrivate ?? false;
        public bool IsGetPrivate => Meta.GetMethod?.IsPrivate ?? false;
        /// <summary>
        /// Can determine if the property is static if either of it's methods are static themselves.
        /// </summary>
        public bool IsStatic => (Meta.GetMethod?.IsStatic ?? false) || (Meta.SetMethod?.IsStatic ?? false);
        /// <summary>
        /// Can determine if the property is virtual if either of it's methods are static themselves.
        /// </summary>
        public bool IsVirtual => (Meta.GetMethod?.IsVirtual ?? false) || (Meta.SetMethod?.IsVirtual ?? false);
        public override string Type => Meta.PropertyType.ToString();

        public PropertyModel(PropertyInfo prop) : base(prop) { }
    }
}
