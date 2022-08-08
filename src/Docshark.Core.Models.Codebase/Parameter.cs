using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Core.Models.Codebase
{
    /// <summary>
    /// Represents a parameter in a function signature.
    /// </summary>
    public struct Parameter
    {
        /// <summary>
        /// Name of the parameter.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Type of the parameter.
        /// </summary>
        public string Type { get; set; }
    }
}
