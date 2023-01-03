using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Core.Models.Mongo.Comments
{
    public record class TypeCommentsModel : CommonCommentsModel<TypeComments>
    {
        public TypeCommentsModel() { }
        public TypeCommentsModel(TypeComments comments, string fullName, Version version) : base(comments, fullName, version) { }
    }
}
