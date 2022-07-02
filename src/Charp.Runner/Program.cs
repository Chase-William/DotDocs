using System;
using System.Collections.Generic;
using System.IO;

using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using Charp.Core;
using Charp.Core.Models;
using Charp.Core.Loaders;


namespace Charp.Runner
{
    internal class Program
    {        
        static void Main(string[] args)
        {
            //var docs = Charper.From(
            //    dllPath: args[0],
            //    xmlPath: args[1],
            //    outputPath: args[2]
            //);

            var docs = Charper.From(
                dllPath: @"C:\Dev\Charp.Core\test\Charp.Test.Data\bin\Debug\net5.0\Charp.Test.Data.dll",
                xmlPath: @"C:\Dev\Charp.Core\test\Charp.Test.Data\bin\Debug\net5.0\Charp.Test.Data.xml",
                outputPath: @"C:\Users\Chase Roth\Desktop"
            );

            docs.Save();
        }
    }
}
