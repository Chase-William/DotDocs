using System;
using System.Reflection;

using LoxSmoke.DocXml;

namespace Docsharp.Core.Models
{
    public abstract class TypeMember<T1, T2> : Member<T1, T2>
        where T1 : TypeInfo
        where T2 : TypeComments
    {
        public abstract bool CanHaveInternalTypes { get; }

        public string Namespace => Meta.Namespace;
        public string FullName => Meta.FullName;

        protected TypeMember(T1 member) : base(member)
        { }
    }
}
