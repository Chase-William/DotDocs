using Docsharp.Core;
using NUnit.Framework;

namespace Docsharp.Test
{
    public class Tests
    {
        public Docsharpener Docs;

        [SetUp]
        public void Setup()
        {
            Docs = Docsharpener.From(
                @"C:\Dev\Sharpocs\test\Docsharp.Test.Data\bin\Debug\net5.0\Docsharp.Test.Data.dll",
                @"C:\Dev\Sharpocs\test\Docsharp.Test.Data\bin\Debug\net5.0\Docsharp.Test.Data.xml");            
        }

        /// <summary>
        /// Ensures models within the namespace "Docsharp.Test.Data.Models.*" are accounted for within the Metadata Tree.
        /// </summary>
        [Test]
        public void ClassesExist()
        {
            Assert.AreEqual(
                "Boat", 
                Docs.Metadata.Root
                .Namespaces["Docsharp"]
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Classes"]
                .Types["Boat"]
                .GetName());
            Assert.AreEqual(
                "Canoe",
                Docs.Metadata.Root
                .Namespaces["Docsharp"]
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Classes"]
                .Types["Canoe"]
                .GetName());
            Assert.AreEqual(
                "Runabout",
                Docs.Metadata.Root
                .Namespaces["Docsharp"]
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Classes"]
                .Types["Runabout"]
                .GetName());
            Assert.AreEqual(
                "Sailboat",
                Docs.Metadata.Root
                .Namespaces["Docsharp"]
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Classes"]
                .Types["Sailboat"]
                .GetName());
            Assert.AreEqual(
                "Yacht",
                Docs.Metadata.Root
                .Namespaces["Docsharp"]
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Classes"]
                .Types["Yacht"]
                .GetName());
        }

        [Test]
        public void ClassesMemberInfoNotNull()
        {
            Assert.NotNull((
                Docs.Metadata.Root
                .Namespaces["Docsharp"]
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Classes"]
                .Types["Boat"] as TypeNode)
                .Member);
            Assert.NotNull((
                Docs.Metadata.Root
                .Namespaces["Docsharp"]
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Classes"]
                .Types["Canoe"] as TypeNode)
                .Member);
            Assert.NotNull((
                Docs.Metadata.Root
                .Namespaces["Docsharp"]
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Classes"]
                .Types["Runabout"] as TypeNode)
                .Member);
            Assert.NotNull((
                Docs.Metadata.Root
                .Namespaces["Docsharp"]
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Classes"]
                .Types["Sailboat"] as TypeNode)
                .Member);
            Assert.NotNull((
                Docs.Metadata.Root
                .Namespaces["Docsharp"]
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Classes"]
                .Types["Yacht"] as TypeNode)
                .Member);
        }

        [Test]
        public void InterfacesExist()
        {
            Assert.AreEqual(
                "IPowerable",
                Docs.Metadata.Root
                .Namespaces["Docsharp"]
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
                Docs.Metadata.Root
                .Namespaces["Docsharp"]
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Enumerations"]
                .Types["EngineSize"]
                .GetName());
        }
    }
}