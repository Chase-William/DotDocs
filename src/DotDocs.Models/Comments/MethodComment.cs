using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models.Comments
{
    public class MethodComment : CommonComment
    {
        public List<(string name, string text)> Parameters { get; set; }
        public List<(string name, string text)> TypeParameters { get; set; }
        public List<(string code, string text)> Responses { get; set; }
        public string Returns { get; set; }
    }
}
