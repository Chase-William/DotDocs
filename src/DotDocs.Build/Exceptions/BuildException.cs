using Microsoft.Build.Logging.StructuredLogger;
using System.Text;

namespace DotDocs.Build.Exceptions
{
    /// <summary>
    /// An exception used when a project's build fails.
    /// </summary>
    public class BuildException : Exception
    {
        /// <summary>
        /// Build errors reported from the build binlog.
        /// </summary>
        public IReadOnlyList<Error> Errors { get; set; }
        /// <summary>
        /// Creates a new instance of the <see cref="BuildException"/> class with errors.
        /// </summary>
        /// <param name="errors">Build errors.</param>
        public BuildException(IReadOnlyList<Error> errors)
            => Errors = errors;

        /// <summary>
        /// <inheritdoc cref="Exception.Message"/>
        /// </summary>
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
