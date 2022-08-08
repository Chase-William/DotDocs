﻿using Docshark.Core.Models.Codebase.Types;
using Docshark.Test.Data.Interfaces;
using NUnit.Framework;

namespace Docshark.Test.Types
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
            => Docs.Builder.ProjectManager.RootProject.Codebase.Root
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Interfaces"]
                .Types[interfaceName]
                .Member as InterfaceModel;
    }
}
