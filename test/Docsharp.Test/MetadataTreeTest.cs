using Docsharp.Core;
using NUnit.Framework;
using System;

namespace Docsharp.Test
{
    internal class Tests : BaseTest
    {      
        
        [Test]
        public void InterfacesExist()
        {
            Assert.AreEqual(
                "IPowerable",
                Docs.ModelTree.Root
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
                Docs.ModelTree.Root
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Enumerations"]
                .Types["EngineSize"]
                .GetName());
        }        
    }
}