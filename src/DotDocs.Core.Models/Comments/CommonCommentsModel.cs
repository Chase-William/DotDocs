using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Core.Models.Comments
{
    public class CommonCommentsModel<TComment> where TComment : CommonComments
    {
        T comments;

        public CommonCommentsModel() { }

        public int MyProperty { get; set; }

        public CommonCommentsModel(TComment comments)
        {
            this.comments = comments;           
        }
    }
}
