using System;
using Docshark.Core;
using Docshark.Core.Exceptions;
using System.Linq;
using System.Text.Json;

namespace Docshark.Runner
{
    internal class Program
    {        
        static void Main(string[] args)
        {
            Run(csProjFile: args[0],
                outputPath: args[1]);

            //var docs = Charper.From(
            //    csProjPath: @"C:\Dev\Charp.Core\src\Charp.Core\Charp.Core.csproj",
            //    dllPath: @"C:\Dev\Charp.Core\src\Charp.Core\bin\Debug\net6.0\Charp.Core.dll",
            //    xmlPath: @"C:\Dev\Charp.Core\src\Charp.Core\bin\Debug\net6.0\Charp.Core.xml",
            //    outputPath: @"C:\Users\Chase Roth\Desktop"
            //);

            // Test project in entire diff dir
            //Run(csProjFile: @"C:\Dev\ProjDepResolver\src\ProjDepResolver\ProjDepResolver.csproj",
            //    outputPath: @"C:\Users\Chase Roth\Desktop");

            // Test test library
            //Run(csProjFile: @"C:\Dev\Docshark.Core\test\Docshark.Test.Data\Docshark.Test.Data.csproj",
            //    outputPath: @"C:\Users\Chase Roth\Desktop");

            // Test test library
            //Run(csProjFile: @"C:\Dev\Docshark.Core\test\Docshark.Test.Standard\Docshark.Test.Standard.csproj",
            //    outputPath: @"C:\Users\Chase Roth\Desktop");
        }

        static void Run(string csProjFile, string outputPath)
        {
            try
            {
                using var docs = new Docsharker(
                    csProjFile: csProjFile,
                    outputPath: outputPath
                );

                docs.Prepare();
                docs.Load();
                docs.Make();
            }
            catch (BuildException ex)
            {
                var test = ex.Errors.Select(err => new Error
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
                });
                Console.WriteLine(JsonSerializer.Serialize(test));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        class Error
        {
            public DateTime Timestamp { get; set; }
            public string Code { get; set; }
            public int ColumnNumber { get; set; }
            public int EndColumnNumber { get; set; }
            public int EndLineNumber { get; set; }
            public string File { get; set; }
            public int LineNumber { get; set; }
            public string ProjectFile { get; set; }
            public string Subcategory { get; set; }
        }
    }
}
