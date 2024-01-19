using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Source
{
    internal class LocalSource : ISourceable
    {
        public string Src { get; init; }

        public LocalSource(string src)
            => Src = src;

        /// <summary>
        /// Prepares/Ensures local files are ready for usage.
        /// </summary>
        /// <returns>Self</returns>
        /// <exception cref="DirectoryNotFoundException">Throws if <see cref="Src"/> directory does not exist.</exception>
        public ISourceable Prepare()
        {
            if (!Directory.Exists(Src))
                throw new DirectoryNotFoundException(Src);

            return this;
        }
    }
}
