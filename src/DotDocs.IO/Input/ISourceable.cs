using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.IO
{
    public interface ISourceable
    {
        string Src { get; init; }

        /// <summary>
        /// Returns the <see cref="Src"/>.
        /// </summary>
        /// <returns></returns>
        string GetSource() => Src;

        /// <summary>
        /// Prepares the source for usage.
        /// </summary>
        /// <returns></returns>
        ISourceable Prepare();
    }
}
