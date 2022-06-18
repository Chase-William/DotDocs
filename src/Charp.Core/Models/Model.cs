using System.Reflection;
using System.Text.Json.Serialization;

using LoxSmoke.DocXml;

namespace Charp.Core.Models
{
    public abstract class Model<T1, T2>
        where T1 : MemberInfo
        where T2 : CommonComments
    {
        [JsonIgnore]
        public T1 Meta { get; private set; }

        public T2 Comments { get; set; }

        protected Model(T1 member)
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
