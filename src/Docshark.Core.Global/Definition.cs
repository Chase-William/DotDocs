using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Core.Global
{
    /// <summary>
    /// Enforces standards upon all other definitions which inherit from this type.  
    /// </summary>
    public abstract class Definition
    {
        /// <summary>
        /// The primary identifer (id) for a <see cref="Definition"/>.
        /// </summary>
        public abstract string GetPrimaryKey();
    }
}
