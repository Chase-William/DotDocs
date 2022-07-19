using Docshark.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Test
{
    internal class BaseTest
    {
        public static Charper Docs;

        /// <summary>
        /// OneTimeSetup is required because having multiple active MetadataLoadContexts 
        /// in parallel test is forbidden.
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            Docs = Charper.From(
                csProjPath: @"C:\Dev\Docshark.Core\test\Docshark.Test.Data\Docshark.Test.Data.csproj",
                dllPath: @"C:\Dev\Docshark.Core\test\Docshark.Test.Data\bin\Debug\net6.0\Docshark.Test.Data.dll",
                xmlPath: @"C:\Dev\Docshark.Core\test\Docshark.Test.Data\bin\Debug\net6.0\Docshark.Test.Data.xml",
                outputPath: "");
        }

        [OneTimeTearDown]
        public void TearDown()
            => Docs.Dispose();

        public static string GetTypeTestMessage(Type type)
            => $"Type Tested: {type.FullName}";
    }
}
