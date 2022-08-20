using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Core.Loader.Exceptions
{
    public class BuildException : Exception
    {
        public IReadOnlyList<Error> Errors { get; set; }

        public BuildException(IReadOnlyList<Error> errors)
            => Errors = errors;

        public override string Message
        {
            get
            {
                var builder = new StringBuilder();
                foreach (var err in Errors)
                    builder.AppendLine(err.ToString());
                return builder.ToString();
            }
        }
    }
}
