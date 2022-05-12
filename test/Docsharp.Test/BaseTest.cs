using Docsharp.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docsharp.Test
{
    internal class BaseTest
    {
        public Docsharpener Docs;

        /// <summary>
        /// OneTimeSetup is required because having multiple active MetadataLoadContexts 
        /// in parallel test is forbidden.
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            Docs = Docsharpener.From(
                @"C:\Dev\Sharpocs\test\Docsharp.Test.Data\bin\Debug\net5.0\Docsharp.Test.Data.dll",
                @"C:\Dev\Sharpocs\test\Docsharp.Test.Data\bin\Debug\net5.0\Docsharp.Test.Data.xml");
        }

        [OneTimeTearDown]
        public void TearDown()
            => Docs.Dispose();

        public static string GetTypeTestMessage(Type type)
            => $"Type Tested: {type.FullName}";
    }
}
