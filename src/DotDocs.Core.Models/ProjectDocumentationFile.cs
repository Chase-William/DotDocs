using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Core.Models
{
    public class ProjectDocumentationFile
    {
        public string? FilePath { get; set; }
        public string? GitHash { get; set; }
        public Version? Version { get; set; }
    }
}
