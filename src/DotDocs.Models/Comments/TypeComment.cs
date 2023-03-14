using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models.Comments
{
    public class TypeComment : CommonComment
    {
        public List<(string name, string text)> Parameters { get; set; }
    }
}
