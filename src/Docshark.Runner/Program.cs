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
            //var docs = Charper.From(
            //    csProjPath: args[0],
            //    dllPath: args[1],
            //    xmlPath: args[2],
            //    outputPath: args[3]
            //);

            //var docs = Charper.From(
            //    csProjPath: @"C:\Dev\Charp.Core\src\Charp.Core\Charp.Core.csproj",
            //    dllPath: @"C:\Dev\Charp.Core\src\Charp.Core\bin\Debug\net6.0\Charp.Core.dll",
            //    xmlPath: @"C:\Dev\Charp.Core\src\Charp.Core\bin\Debug\net6.0\Charp.Core.xml",
            //    outputPath: @"C:\Users\Chase Roth\Desktop"
            //);

            var docs = Charper.From(
                csProjPath: @"C:\Dev\Charp.Core\test\Charp.Test.Data\Charp.Test.Data.csproj",
                dllPath: @"C:\Dev\Charp.Core\test\Charp.Test.Data\bin\Debug\net6.0\Charp.Test.Data.dll",
                xmlPath: @"C:\Dev\Charp.Core\test\Charp.Test.Data\bin\Debug\net6.0\Charp.Test.Data.xml",
                outputPath: @"C:\Users\Chase Roth\Desktop"
            );

            docs.Save();
        }
    }
}
