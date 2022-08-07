using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Core.Models.Project
{
    /// <summary>
    /// Represents a dependency that has a dll and a documentation file.
    /// </summary>
    public class Dependency
    {
        public string AssemblyName { get; set; }
        public string AssemblyPath { get; set; }        
        public string? DocumentationPath { get; set; }
    }
}
