using System;
using System.Reflection;

using LoxSmoke.DocXml;

namespace Charp.Core.Models.Types
{
    public abstract class TypeMember<T1, T2> : Model<T1, T2>
        where T1 : TypeInfo
        where T2 : TypeComments
    {
        public abstract bool CanHaveInternalTypes { get; }

        public string Namespace => Meta.Namespace;
        public string FullName => Meta.FullName;

        public bool IsPublic => Meta.IsPublic || Meta.IsNestedPublic;
        public bool IsInternal => !Meta.IsVisible;
        public string Parent => Meta.BaseType?.ToString();

        protected TypeMember(T1 member) : base(member)
        { }
    }
}
