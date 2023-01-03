using System.Reflection;
using System.Text.Json.Serialization;
using DotDocs.Core.Models.Mongo.Comments;
using LoxSmoke.DocXml;

namespace DotDocs.Core.Models.Language.Members
{
    public class PropertyModel : MemberModel<PropertyInfo, CommonCommentsModel<CommonComments>>
    {
        #region Individual Get & Set Info
        public bool HasGetter => Info.CanRead;
        public bool HasSetter => Info.CanWrite;
        public bool? IsGetPublic => Info.GetMethod?.IsPublic;
        public bool? IsSetPublic => Info.SetMethod?.IsPublic;
        public bool? IsGetPrivate => Info.GetMethod?.IsPrivate;
        public bool? IsSetPrivate => Info.SetMethod?.IsPrivate;
        public bool? IsGetProtected
        {
            get
            {
                if (Info.GetMethod?.Attributes == null)
                    return null;
                return Info.GetMethod.IsFamily || Info.GetMethod.IsFamilyOrAssembly;
            }
        }
        public bool? IsSetProtected
        {
            get
            {
                if (Info.SetMethod?.Attributes == null)
                    return null;
                return Info.SetMethod.IsFamily || Info.SetMethod.IsFamilyOrAssembly;
            }
        }

        public bool? IsGetInternal
        {
            get
            {
                if (Info.GetMethod?.Attributes == null)
                    return null;
                return Info.GetMethod.IsAssembly || Info.GetMethod.IsFamilyOrAssembly;
            }
        }
        public bool? IsSetInternal
        {
            get
            {
                if (Info.SetMethod?.Attributes == null)
                    return null;
                return Info.SetMethod.IsAssembly || Info.SetMethod.IsFamilyOrAssembly;
            }
        }
        #endregion            

        #region Overall Info
        /// <summary>
        /// Determines if the property is static if either of it's methods are static themselves.
        /// </summary>
        public bool IsStatic => (Info.GetMethod?.IsStatic ?? true) || (Info.SetMethod?.IsStatic ?? true);
        /// <summary>
        /// Determines if the property is virtual if either of it's methods are virtual themselves.
        /// </summary>
        public bool IsVirtual
            => !(Info.GetMethod?.Attributes.HasFlag(MethodAttributes.Abstract) ?? false) && (Info.GetMethod?.Attributes.HasFlag(MethodAttributes.Virtual) ?? false) ||
               !(Info.SetMethod?.Attributes.HasFlag(MethodAttributes.Abstract) ?? false) && (Info.SetMethod?.Attributes.HasFlag(MethodAttributes.Virtual) ?? false);
        /// <summary>
        /// Determines if the propert is abstract if either of it's methods are abstract themselves.
        /// </summary>
        public bool IsAbstract
            => (Info.GetMethod?.Attributes.HasFlag(MethodAttributes.Abstract) ?? false) && (Info.GetMethod?.Attributes.HasFlag(MethodAttributes.Virtual) ?? false) ||
               (Info.SetMethod?.Attributes.HasFlag(MethodAttributes.Abstract) ?? false) && (Info.SetMethod?.Attributes.HasFlag(MethodAttributes.Virtual) ?? false);

        #region Accessibility
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

        public string Type => Info.PropertyType.GetTypeId();        

        public PropertyModel(PropertyInfo info) : base(info) { }
    }
}
