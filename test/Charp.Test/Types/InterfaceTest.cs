using Charp.Core.Models.Types;
using Charp.Test.Data.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charp.Test.Types
{
    internal class InterfaceTest : BaseTest
    {
        [Test(Description = "Ensures the <IsPublic> member of the <InterfaceModel> type is set correctly.")]
        public void IsPublicSetCorrectly()
        {
            Assert.IsTrue(GetInterfaceModel(nameof(IPowerable)).IsPublic);
        }

        [Test(Description = "Ensures the <IsInternal> member of the <InterfaceModel> type is set correctly.")]
        public void IsInternalSetCorrectly()
        {
            Assert.IsFalse(GetInterfaceModel(nameof(IPowerable)).IsInternal);
        }

        public static InterfaceModel GetInterfaceModel(string interfaceName)
            => Docs.Models.Root
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Interfaces"]
                .Types[interfaceName]
                .Member as InterfaceModel;
    }
}
