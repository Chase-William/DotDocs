using System;
using System.Reflection;

using Docsharp.Core.Metadata;

namespace Docsharp.Core.Types
{
    public abstract class TypeMember<T1, T2> : Member<T1, T2>, ITest
        where T1 : TypeInfo
        where T2 : Documentation
    {
        public abstract bool CanHaveInternalTypes { get; }

        protected TypeMember(T1 member) : base(member)
        { }       
    }

    public interface ITest
    {

    }
}
