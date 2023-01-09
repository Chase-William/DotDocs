using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Core.Models.Exceptions
{
    /// <summary>
    /// An exception that is used when the root or a .csproj file referenced by another project cannot be found.
    /// </summary>
    public class MissingProjectFileException : Exception
    {
        /// <summary>
        /// The project file that cannot be found.
        /// </summary>
        public string ProjectFile { get; set; }
        /// <summary>
        /// Creates a new instance of the <see cref="MissingProjectFileException"/> class with a missing file path.
        /// </summary>
        /// <param name="csProjFile"></param>
        public MissingProjectFileException(string csProjFile)
            => ProjectFile = csProjFile;
        /// <summary>
        /// <inheritdoc cref="Message"/>
        /// </summary>
        public override string Message => "Was unable to locate the following file: '" + ProjectFile + "'.";
    }
}
