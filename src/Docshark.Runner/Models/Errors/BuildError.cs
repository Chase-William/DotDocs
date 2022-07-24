using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Runner.Models.Errors
{
    internal class BuildError
    {
        public DateTime Timestamp { get; set; }
        public string Code { get; set; }
        public int ColumnNumber { get; set; }
        public int EndColumnNumber { get; set; }
        public int EndLineNumber { get; set; }
        public string File { get; set; }
        public int LineNumber { get; set; }
        public string ProjectFile { get; set; }
        public string Subcategory { get; set; }
    }
}
