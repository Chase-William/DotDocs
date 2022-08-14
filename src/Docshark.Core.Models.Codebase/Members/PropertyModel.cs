using System.Reflection;
using Docshark.Core.Models.Codebase;
using LoxSmoke.DocXml;
using Docshark.Core.Models;

namespace Docshark.Core.Models.Codebase.Members
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyModel : MemberModel<PropertyInfo, CommonComments>, IAccessible
    {
        #region Individual Get & Set Info
        public bool HasGetter => Meta.CanRead;
        public bool HasSetter => Meta.CanWrite;
        public bool? IsGetPublic => Meta.GetMethod?.IsPublic;
        public bool? IsSetPublic => Meta.SetMethod?.IsPublic;
        public bool? IsGetPrivate => Meta.GetMethod?.IsPrivate;
        public bool? IsSetPrivate => Meta.SetMethod?.IsPrivate;
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
        #endregion            

        #region Overall Info
        /// <summary>
        /// Determines if the property is static if either of it's methods are static themselves.
        /// </summary>
        public bool IsStatic => (Meta.GetMethod?.IsStatic ?? true) || (Meta.SetMethod?.IsStatic ?? true);
        /// <summary>
        /// Determines if the property is virtual if either of it's methods are virtual themselves.
        /// </summary>
        public bool IsVirtual 
            => (!(Meta.GetMethod?.Attributes.HasFlag(MethodAttributes.Abstract) ?? false) && (Meta.GetMethod?.Attributes.HasFlag(MethodAttributes.Virtual) ?? false)) ||
               (!(Meta.SetMethod?.Attributes.HasFlag(MethodAttributes.Abstract) ?? false) && (Meta.SetMethod?.Attributes.HasFlag(MethodAttributes.Virtual) ?? false));        
        /// <summary>
        /// Determines if the propert is abstract if either of it's methods are abstract themselves.
        /// </summary>
        public bool IsAbstract
            => ((Meta.GetMethod?.Attributes.HasFlag(MethodAttributes.Abstract) ?? false) && (Meta.GetMethod?.Attributes.HasFlag(MethodAttributes.Virtual) ?? false)) ||
               ((Meta.SetMethod?.Attributes.HasFlag(MethodAttributes.Abstract) ?? false) && (Meta.SetMethod?.Attributes.HasFlag(MethodAttributes.Virtual) ?? false));        

        #region IAccessible
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
        public bool IsPrivate => (IsGetPrivate ?? false) || (IsSetPrivate ?? false);
        #endregion
        #endregion

        public PropertyModel(PropertyInfo prop) : base(prop)
        {
            SetType(prop.PropertyType);
        }
    }
}
