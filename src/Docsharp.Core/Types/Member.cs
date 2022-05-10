using System;
using System.Collections.Generic;
using System.Reflection;

namespace Docsharp.Core.Types
{
    public abstract class Member<T> where T : MemberInfo
    {
        protected T member;

        protected Member(T member)
            => this.member = member;

        public string Name => member.Name;        

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
