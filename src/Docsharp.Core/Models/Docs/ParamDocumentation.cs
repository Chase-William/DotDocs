using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docsharp.Core.Models.Docs
{
    /// <summary>
    /// Represents documentation about a parameter.
    /// </summary>
    public class ParamDocumentation
    {
        /// <summary>
        /// Name of the parameter.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Parameter summary.
        /// </summary>
        public string Summary { get; set; }
    }
}
