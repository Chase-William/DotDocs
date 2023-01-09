using LoxSmoke.DocXml;
using System.Reflection;

namespace DotDocs.Core.Comments
{
    /// <summary>
    /// Extension of <see cref="CommonComments"/> for database interaction.
    /// </summary>
    /// <typeparam name="TComment"></typeparam>
    public record class CommonCommentsModel<TComment> : CommentsModel<TComment> where TComment : CommonComments
    {
        /// <summary>
        /// The <see cref="Type.FullName"/> of the encapsolating type.
        /// </summary>
        public string DeclaringTypeFullName { get; init; }

        /// <summary>
        /// The name of the member.
        /// </summary>
        public string Name { get; init; }

        public CommonCommentsModel(TComment comments, MemberInfo info, Version version) : base(comments, version)
        {
            DeclaringTypeFullName = info.DeclaringType.FullName;
            Name = info.Name;
        }
    }
}
