using System;
using System.Reflection;
using Charp.Core.Models.Members;
using LoxSmoke.DocXml;

namespace Charp.Core.Models.Types
{
    /// <summary>
    /// Represents a type.
    /// </summary>
    /// <typeparam name="T1">Information about the type.</typeparam>
    /// <typeparam name="T2">Written documentation.</typeparam>
    public abstract class TypeMember<T1, T2> : Model<T1, T2>, IAccessible
        where T1 : TypeInfo
        where T2 : TypeComments
    {
        /// <summary>
        /// Denotes whether this type can have internal types.
        /// </summary>
        public abstract bool CanHaveInternalTypes { get; }

        /// <summary>
        /// Gets the namespace of this type.
        /// </summary>
        public string Namespace => Meta.Namespace;
        /// <summary>
        /// Gets the namespace with the name of this type.
        /// </summary>
        public string FullName => Meta.FullName;
        
        /// <summary>
        /// Gets the parent of this type.
        /// </summary>
        public string Parent => Meta.BaseType?.ToString();

        #region IAccessible
        /// <summary>
        /// Denotes if this type is public.
        /// </summary>
        public bool IsPublic => Meta.IsPublic || Meta.IsNestedPublic;
        /// <summary>
        /// Denotes if this type is internal.
        /// </summary>
        public bool IsInternal => (Meta.IsNotPublic && !Meta.IsNested) || Meta.IsNestedAssembly || Meta.IsNestedFamORAssem || Meta.IsNestedFamANDAssem;
        /// <summary>
        /// Denotes if this type is private.
        /// </summary>
        public bool IsPrivate => Meta.IsNestedPrivate;
        /// <summary>
        /// Denotes if this type is protected.
        /// </summary>
        public bool IsProtected => Meta.IsNestedFamily || Meta.IsNestedFamANDAssem || Meta.IsNestedFamORAssem;
        #endregion

        /// <summary>
        /// Initializes a new instance of <see cref="TypeMember{T1, T2}"/>.
        /// </summary>
        /// <param name="member">Information about the type.</param>
        protected TypeMember(T1 member) : base(member)
        { }
    }
}
