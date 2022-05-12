using System;
using System.Reflection;

using Docsharp.Core.Models.Docs;

namespace Docsharp.Core.Models
{
    public abstract class TypeMember<T1, T2> : Member<T1, T2>
        where T1 : TypeInfo
        where T2 : Documentation
    {
        public abstract bool CanHaveInternalTypes { get; }

        public string Namespace => Meta.Namespace;
        public string FullName => Meta.FullName;

        protected TypeMember(T1 member) : base(member)
        { }
    }
}
