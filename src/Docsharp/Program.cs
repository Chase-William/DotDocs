using System;
using System.Collections.Generic;
using System.IO;

using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using Docsharp.Core;
using Docsharp.Core.Models;
using Docsharp.Core.Loaders;


namespace Docsharp
{
    internal class Program
    {        
        static void Main(string[] args)
        {
            var docs = Docsharpener.From(
                dllPath: args[0],
                xmlPath: args[1]
            );

            //var docs = Docsharpener.From(
            //    dllPath: @"C:\Dev\Sharpocs\test\Docsharp.Test.Data\bin\Debug\net5.0\Docsharp.Test.Data.dll",
            //    xmlPath: @"C:\Dev\Sharpocs\test\Docsharp.Test.Data\bin\Debug\net5.0\Docsharp.Test.Data.xml"
            //);

            docs.Save();
        }
    }
}
