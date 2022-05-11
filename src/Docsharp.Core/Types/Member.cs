using Docsharp.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Docsharp.Core.Types
{
    public abstract class Member<T1, T2> 
        where T1 : MemberInfo 
        where T2 : Documentation
    {
        public T1 TypeInfo { get; private set; }

        public T2 Docs { get; set; }

        protected Member(T1 member)
            => this.TypeInfo = member;

        public string Name => TypeInfo.Name;

        public abstract string Type { get; }

        // public Module Module => member.Module;
        //public int MetadataToken => member.MetadataToken;
        //public MemberTypes MemberTypes => member.MemberType;
        //public bool IsCollectable => member.IsCollectible;

        // public Type? DeclaringType => member.DeclaringType;
        // public IEnumerable<CustomAttributeData> CustomAttributes => member.CustomAttributes;        
        // public Type? ReflectedType => member.ReflectedType;
    }
}
