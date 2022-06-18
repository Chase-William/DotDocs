using System.Reflection;

using LoxSmoke.DocXml;

namespace Charp.Core.Models.Members
{
    public class PropertyModel : Model<PropertyInfo, CommonComments>, IMemberable
    {
        #region Individual Get & Set Info
        public bool HasGetter => Meta.CanRead;
        public bool HasSetter => Meta.CanWrite;
        public bool? IsGetPublic => !Meta.GetMethod?.IsPrivate;
        public bool? IsSetPublic => !Meta.SetMethod?.IsPrivate;
        public bool? IsGetProtected
        {
            get
            {
                if (Meta.GetMethod?.Attributes == null)
                    return null;
                return Meta.GetMethod.IsFamily || Meta.GetMethod.IsFamilyOrAssembly;
            }
        }
        public bool? IsSetProtected
        {
            get
            {
                if (Meta.SetMethod?.Attributes == null)
                    return null;
                return Meta.SetMethod.IsFamily || Meta.SetMethod.IsFamilyOrAssembly;
            }
        }

        public bool? IsGetInternal
        {
            get
            {
                if (Meta.GetMethod?.Attributes == null)
                    return null;
                return Meta.GetMethod.IsAssembly || Meta.GetMethod.IsFamilyOrAssembly;
            }
        }            
        public bool? IsSetInternal
        {
            get
            {
                if (Meta.SetMethod?.Attributes == null)
                    return null;
                return Meta.SetMethod.IsAssembly || Meta.SetMethod.IsFamilyOrAssembly;
            }
        }
        //public MethodInfo Test => Meta.GetMethod;
        #endregion            

        #region Overall Info
        /// <summary>
        /// Determines if the property is static if either of it's methods are static themselves.
        /// </summary>
        public bool IsStatic => (Meta.GetMethod?.IsStatic ?? false) || (Meta.SetMethod?.IsStatic ?? false);
        /// <summary>
        /// Determines if the property is virtual if either of it's methods are static themselves.
        /// </summary>
        public bool IsVirtual => (Meta.GetMethod?.IsVirtual ?? false) || (Meta.SetMethod?.IsVirtual ?? false);
        public override string Type => Meta.PropertyType.ToString();

        #region IMemberable
        /// <summary>
        /// Determines if the property is protected as a whole. This means both the set and get methods are protected if present.
        /// </summary>
        public bool IsProtected
            => (IsGetProtected ?? true) && (IsSetProtected ?? true);
        /// <summary>
        /// Determines if the property is internal as a whole. This means both the set and get methods are internal if present.
        /// </summary>
        public bool IsInternal 
            => (IsGetInternal ?? true) && (IsSetInternal ?? true);
        /// <summary>
        /// Determines if the property is publicly accessible to any degree.
        /// </summary>
        public bool IsPublic => (IsGetPublic ?? false) || (IsSetPublic ?? false);
        #endregion
        #endregion

        public PropertyModel(PropertyInfo prop) : base(prop) { }
    }
}
