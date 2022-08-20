using DotDocs.Core.Mapper.Project.Exceptions;
using DotDocs.Runner.Models.Errors;
using System.Linq;
using System.Text.Json;

namespace DotDocs.Runner
{
    public static class Extensions
    {
        public static string Stringify(this BuildException ex)
        {
            object root = new
            {
                Errors = ex.Errors.Select(err => new BuildError
                {
                    Timestamp = err.Timestamp,
                    Code = err.Code,
                    ColumnNumber = err.ColumnNumber,
                    EndColumnNumber = err.EndColumnNumber,
                    EndLineNumber = err.EndLineNumber,
                    File = err.File,
                    LineNumber = err.LineNumber,
                    ProjectFile = err.ProjectFile,
                    Subcategory = err.Subcategory
                }).ToArray()
            };

            return JsonSerializer.Serialize(root);
        }

        public static string Stringify(this MissingProjectFileException ex)
        {
            object root = new
            {
                ProjectFile = ex.ProjectFile
            };

            return JsonSerializer.Serialize(root);
        }
    }
}
