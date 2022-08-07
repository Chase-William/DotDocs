using System;
using Docshark.Core;
using Docshark.Core.Exceptions;

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
            //Run(csProjFile: @"C:\Dev\Docshark.Core\src\Docshark.Core\Docshark.Core.csproj",
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
                Console.WriteLine(((int)ErrorCodes.UserError) + " " + ex.Message);
            }
            catch (MissingProjectFileException ex)
            {
                Console.WriteLine(((int)ErrorCodes.UserError) + " " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine((int)ErrorCodes.InternalError + " " + ex.Message);
            }
        }
    }
}
