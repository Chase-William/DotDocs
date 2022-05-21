using Charp.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charp.Test
{
    internal class BaseTest
    {
        public Charper Docs;

        /// <summary>
        /// OneTimeSetup is required because having multiple active MetadataLoadContexts 
        /// in parallel test is forbidden.
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            Docs = Charper.From(
                @"C:\Dev\Charp\test\Charp.Test.Data\bin\Debug\net5.0\Charp.Test.Data.dll",
                @"C:\Dev\Charp\test\Charp.Test.Data\bin\Debug\net5.0\Charp.Test.Data.xml");
        }

        [OneTimeTearDown]
        public void TearDown()
            => Docs.Dispose();

        public static string GetTypeTestMessage(Type type)
            => $"Type Tested: {type.FullName}";
    }
}
