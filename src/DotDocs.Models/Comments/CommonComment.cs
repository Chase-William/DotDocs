using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models.Comments
{
    public class CommonComment : Model
    {
        public string Summary { get; set; }
        public string Example { get; set; }
        public string Remarks { get; set; }
    }
}
