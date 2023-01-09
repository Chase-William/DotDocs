using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Core.Models.Comments
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
