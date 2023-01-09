using LoxSmoke.DocXml;

namespace DotDocs.Core.Comments
{
    public record class TypeCommentsModel : CommentsModel<TypeComments>
    {
        public string FullName { get; init; }

        public TypeCommentsModel(TypeComments comments, Type type, Version version) : base(comments, version)
        {
            this.FullName = type.FullName;
        }
    }
}
