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
        public static Docsharker Docs;

        /// <summary>
        /// OneTimeSetup is required because having multiple active MetadataLoadContexts 
        /// in parallel test is forbidden.
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            try
            {
                Docs = new Docsharker(
                    csProjFile: @"C:\Dev\Docshark.Core\test\Docshark.Test.Data\Docshark.Test.Data.csproj",
                    outputPath: @"C:\Users\Chase Roth\Desktop"
                );

                Docs.Prepare();
                Docs.Load();
                Docs.Make();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Docs = null;
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            if (Docs != null)
                Docs.Dispose();
        }

        public static string GetTypeTestMessage(Type type)
            => $"Type Tested: {type.FullName}";
    }
}
