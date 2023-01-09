using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DotDocs.Core.Models.Comments
{
    /// <summary>
    /// Extension of <see cref="CommonCommentsModel{TComment}"/> for interacting with the database.
    /// </summary>
    public record class MethodCommentsModel : CommonCommentsModel<MethodComments>
    {
        string? returns;
        public string? Returns
        {
            get
            {
                return returns ?? comments?.Returns;
            }
            init
            {
                returns = value;
            }
        }

        public MethodCommentsModel(MethodComments comments, MethodInfo method, Version version) : base(comments, method, version) { }
    }
}
