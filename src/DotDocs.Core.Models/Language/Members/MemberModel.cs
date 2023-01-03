using LoxSmoke.DocXml;
using System.Reflection;
using System.Text.Json.Serialization;

namespace DotDocs.Core.Models.Language.Members
{
    public class MemberModel<T1, T2> : Model
        where T1 : MemberInfo
        
    {
        [JsonIgnore]
        public T1 Info { get; set; }

        public T2? Comments { get; set; }

        /// <summary>
        /// The id of the delcaring type for this member. The declaring type is 
        /// the type the member is defined in. For example, a custom class has a .Equals()
        /// method, but until that custom class implements it's own. It will use the one defined
        /// in it's parent that provides .Equals(). That same .Equals() will denote it's declaring type to
        /// be that parent class as that parent is where it "resides".
        /// </summary>
        public string? DeclaringType => Info.DeclaringType?.GetTypeId();

        public override string Name => Info.Name;

        protected MemberModel(T1 info)
            => Info = info;
    }
}
