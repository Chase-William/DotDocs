using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace Docsharp.Core.Types
{
    public abstract class TypeMember<T> : Member<T> where T : TypeInfo
    {
        public abstract bool CanHaveInternalTypes { get; }

        protected TypeMember(T member) : base(member)
        { }
    }
}
