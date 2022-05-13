using System.Reflection;
using System.Text.Json.Serialization;

using Docsharp.Core.Models.Docs;

namespace Docsharp.Core.Models
{
    public abstract class Member<T1, T2>
        where T1 : MemberInfo
        where T2 : Documentation
    {
        [JsonIgnore]
        public T1 Meta { get; private set; }

        public T2 Docs { get; set; }

        protected Member(T1 member)
            => Meta = member;

        public string Name => Meta.Name;

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
