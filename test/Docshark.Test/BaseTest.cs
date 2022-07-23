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
        public static Core.Docshark Docs;

        /// <summary>
        /// OneTimeSetup is required because having multiple active MetadataLoadContexts 
        /// in parallel test is forbidden.
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            Docs = Core.Docshark.From(
                csProjPath: @"C:\Dev\Docshark.Core\test\Docshark.Test.Data\Docshark.Test.Data.csproj",
                outputPath: "");
        }

        [OneTimeTearDown]
        public void TearDown()
            => Docs.Dispose();

        public static string GetTypeTestMessage(Type type)
            => $"Type Tested: {type.FullName}";
    }
}
