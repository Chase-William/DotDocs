using System;
using System.Reflection;
using Charp.Core.Models.Members;
using LoxSmoke.DocXml;

namespace Charp.Core.Models.Types
{
    public abstract class TypeMember<T1, T2> : Model<T1, T2>, IAccessible
        where T1 : TypeInfo
        where T2 : TypeComments
    {
        public abstract bool CanHaveInternalTypes { get; }

        public string Namespace => Meta.Namespace;
        public string FullName => Meta.FullName;
        
        public string Parent => Meta.BaseType?.ToString();

        #region IAccessible
        public bool IsPublic => Meta.IsPublic || Meta.IsNestedPublic;
        public bool IsInternal => (Meta.IsNotPublic && !Meta.IsNested) || Meta.IsNestedAssembly || Meta.IsNestedFamORAssem || Meta.IsNestedFamANDAssem;
        public bool IsPrivate => Meta.IsNestedPrivate;
        public bool IsProtected => Meta.IsNestedFamily || Meta.IsNestedFamANDAssem || Meta.IsNestedFamORAssem;
        #endregion

        protected TypeMember(T1 member) : base(member)
        { }
    }
}
