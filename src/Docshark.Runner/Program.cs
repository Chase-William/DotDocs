using System;
using System.Collections.Generic;
using System.IO;

using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using Docshark.Core;
using Docshark.Core.Models;
using Docshark.Core.Loaders;
using Docshark.Core.Exceptions;
using System.Text.Json.Serialization;

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
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
