using System;
using System.Collections.Generic;
using System.IO;

using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using Charp.Core;
using Charp.Core.Models;
using Charp.Core.Loaders;


namespace Charp
{
    internal class Program
    {        
        static void Main(string[] args)
        {
            //var docs = Charper.From(
            //    dllPath: args[0],
            //    xmlPath: args[1]
            //);

            var docs = Charper.From(
                dllPath: @"C:\Dev\Charp\test\Charp.Test.Data\bin\Debug\net5.0\Charp.Test.Data.dll",
                xmlPath: @"C:\Dev\Charp\test\Charp.Test.Data\bin\Debug\net5.0\Charp.Test.Data.xml"
            );

            docs.Save();
        }
    }
}
