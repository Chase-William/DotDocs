using System;
using System.Collections.Generic;
using System.IO;

using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using Docshark.Core;
using Docshark.Core.Models;
using Docshark.Core.Loaders;


namespace Docshark.Runner
{
    internal class Program
    {        
        static void Main(string[] args)
        {
            //var docs = Core.Docshark.From(
            //    csProjPath: args[0],
            //    outputPath: args[1]
            //);

            //var docs = Charper.From(
            //    csProjPath: @"C:\Dev\Charp.Core\src\Charp.Core\Charp.Core.csproj",
            //    dllPath: @"C:\Dev\Charp.Core\src\Charp.Core\bin\Debug\net6.0\Charp.Core.dll",
            //    xmlPath: @"C:\Dev\Charp.Core\src\Charp.Core\bin\Debug\net6.0\Charp.Core.xml",
            //    outputPath: @"C:\Users\Chase Roth\Desktop"
            //);

            var docs = Core.Docshark.From(
                csProjPath: @"C:\Dev\Docshark.Core\test\Docshark.Test.Data\Docshark.Test.Data.csproj",
                outputPath: @"C:\Users\Chase Roth\Desktop"
            );

            //var docs = Core.Docshark.From(
            //    csProjPath: @"C:\Dev\Docshark.Core\test\Docshark.Test\Docshark.Test.csproj",
            //    outputPath: @"C:\Users\Chase Roth\Desktop"
            //);

            docs.Save();
        }
    }
}
