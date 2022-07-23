using System.Reflection;
using System.Text.Json.Serialization;

using LoxSmoke.DocXml;

namespace Docshark.Core.Models.Lang
{
    /// <summary>
    /// The root base class to all types and members.
    /// </summary>
    /// <typeparam name="T1">Metadata about the type or member.</typeparam>
    /// <typeparam name="T2">Written documentation about the type or member.</typeparam>
    public abstract class Model<T1, T2>
        where T1 : MemberInfo
        where T2 : CommonComments
    {
        /// <summary>
        /// Metadata attained using the MetadataContextLoader.
        /// </summary>
        [JsonIgnore]
        public T1 Meta { get; private set; }

        /// <summary>
        /// Written documentation about the <see cref="Meta"/>.
        /// </summary>
        public T2 Comments { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Model{T1, T2}"/> class with member or type info.
        /// </summary>
        /// <param name="member"></param>
        protected Model(T1 member)
            => Meta = member;

        /// <summary>
        /// Returns the name of the type or member.
        /// </summary>
        public string Name => Meta.Name;

        /// <summary>
        /// Returns the type of the type or member.
        /// </summary>
        public abstract string Type { get; }
    }
}
