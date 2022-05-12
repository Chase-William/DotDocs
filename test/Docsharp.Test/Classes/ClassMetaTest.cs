using Docsharp.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docsharp.Core.Tree;
using Docsharp.Test.Data.Classes;
using System.Reflection;
using Docsharp.Test.Interfaces.Meta;
using Docsharp.Core.Models;

namespace Docsharp.Test.Classes
{
    internal class ClassMetaTest : BaseTest, IConstructableTest
    {
        [Test(Description = "Ensures all class metadata in .dll exist.")]
        public void ClassMetadataExist()
        {
            // Boat
            Assert.NotNull(GetClassType(nameof(Boat)));

            // Canoe
            Assert.NotNull(GetClassType(nameof(Canoe)));

            // Runabout
            Assert.NotNull(GetClassType(nameof(Runabout)));

            // Sailboat
            Assert.NotNull(GetClassType(nameof(Sailboat)));

            // Yacht
            Assert.NotNull(GetClassType(nameof(Yacht)));
        }

        [Test(Description = "Ensures the existance/absense of properties are handled correctly.")]
        public void PropertiesExistTest()
        {
            // Boat
            IConstructable type = GetClassType(nameof(Boat));
            Assert.AreEqual(
                IConstructableTest.GetPropertyCount(typeof(Boat)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Boat)));

            // Canoe
            type = GetClassType(nameof(Canoe));
            Assert.AreEqual(
                IConstructableTest.GetPropertyCount(typeof(Canoe)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Canoe)));

            // Runabout
            type = GetClassType(nameof(Runabout));
            Assert.AreEqual(
                IConstructableTest.GetPropertyCount(typeof(Runabout)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Runabout)));

            // Sailboat
            type = GetClassType(nameof(Sailboat));
            Assert.AreEqual(
                IConstructableTest.GetPropertyCount(typeof(Sailboat)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Sailboat)));

            // Yacht
            type = GetClassType(nameof(Yacht));
            Assert.AreEqual(
                IConstructableTest.GetPropertyCount(typeof(Yacht)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Yacht)));
        }

        [Test(Description = "Ensures all public fields of fields are accounted for.")]
        public void FieldsExistTest()
        {
            // Boat
            IConstructable type = GetClassType(nameof(Boat));
            Assert.AreEqual(
                IConstructableTest.GetFieldCount(typeof(Boat)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Boat)));

            // Canoe
            type = GetClassType(nameof(Canoe));
            Assert.AreEqual(
                IConstructableTest.GetFieldCount(typeof(Canoe)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Canoe)));

            // Runabout
            type = GetClassType(nameof(Runabout));
            Assert.AreEqual(
                IConstructableTest.GetFieldCount(typeof(Runabout)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Runabout)));

            // Sailboat
            type = GetClassType(nameof(Sailboat));
            Assert.AreEqual(
                IConstructableTest.GetFieldCount(typeof(Sailboat)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Sailboat)));

            // Yacht
            type = GetClassType(nameof(Yacht));
            Assert.AreEqual(
                IConstructableTest.GetFieldCount(typeof(Yacht)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Yacht)));
        }

        [Test(Description = "Ensures the existance/absense of methods are handled correctly.")]
        public void MethodsExistTest()
        {
            // Boat
            IConstructable type = GetClassType(nameof(Boat));
            Assert.AreEqual(
                IConstructableTest.GetMethodCount(typeof(Boat)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Boat)));

            // Canoe
            type = GetClassType(nameof(Canoe));
            Assert.AreEqual(
                IConstructableTest.GetMethodCount(typeof(Canoe)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Canoe)));

            // Runabout
            type = GetClassType(nameof(Runabout));
            Assert.AreEqual(
                IConstructableTest.GetMethodCount(typeof(Runabout)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Runabout)));

            // Sailboat
            type = GetClassType(nameof(Sailboat));
            Assert.AreEqual(
                IConstructableTest.GetMethodCount(typeof(Sailboat)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Sailboat)));

            // Yacht
            type = GetClassType(nameof(Yacht));
            Assert.AreEqual(
                IConstructableTest.GetMethodCount(typeof(Yacht)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Yacht)));
        }

        public IConstructable GetClassType(string className)
            => (Docs.Metadata.Root
                .Namespaces["Docsharp"]
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Classes"]
                .Types[className] as TypeNode)
                .Member as IConstructable;
    }
}
