using Docshark.Core;
using NUnit.Framework;
using System;

namespace Docshark.Test
{
    internal class ModelTreeTest : BaseTest
    {      
        
        [Test]
        public void InterfacesExist()
        {
            Assert.AreEqual(
                "IPowerable",
                Docs.Builder.ProjectManager.RootProject.Codebase.Root
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Interfaces"]
                .Types["IPowerable"]
                .GetName());
        }

        [Test]
        public void EnumerationsExist()
        {
            Assert.AreEqual(
                "EngineSize",
                Docs.Builder.ProjectManager.RootProject.Codebase.Root
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Enumerations"]
                .Types["EngineSize"]
                .GetName());
        }        
    }
}