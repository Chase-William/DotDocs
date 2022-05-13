
using NUnit.Framework;

using Docsharp.Core.Tree;
using Docsharp.Core.Models;
using Docsharp.Test.Data.Classes;
using Docsharp.Test.Interfaces.Meta;

namespace Docsharp.Test.Classes
{
    internal class ClassMetaTest : BaseTest, INestableTest
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
            INestable type = GetClassType(nameof(Data.Classes.Boat));
            Assert.AreEqual(
                INestableTest.GetPropertyCount(typeof(Boat)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Boat)));

            // Canoe
            type = GetClassType(nameof(Canoe));
            Assert.AreEqual(
                INestableTest.GetPropertyCount(typeof(Canoe)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Canoe)));

            // Runabout
            type = GetClassType(nameof(Runabout));
            Assert.AreEqual(
                INestableTest.GetPropertyCount(typeof(Runabout)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Runabout)));

            // Sailboat
            type = GetClassType(nameof(Sailboat));
            Assert.AreEqual(
                INestableTest.GetPropertyCount(typeof(Sailboat)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Sailboat)));

            // Yacht
            type = GetClassType(nameof(Yacht));
            Assert.AreEqual(
                INestableTest.GetPropertyCount(typeof(Yacht)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Yacht)));
        }

        [Test(Description = "Ensures all public fields of fields are accounted for.")]
        public void FieldsExistTest()
        {
            // Boat
            Core.Models.INestable type = GetClassType(nameof(Data.Classes.Boat));
            Assert.AreEqual(
                INestableTest.GetFieldCount(typeof(Boat)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Boat)));

            // Canoe
            type = GetClassType(nameof(Canoe));
            Assert.AreEqual(
                INestableTest.GetFieldCount(typeof(Canoe)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Canoe)));

            // Runabout
            type = GetClassType(nameof(Runabout));
            Assert.AreEqual(
                INestableTest.GetFieldCount(typeof(Runabout)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Runabout)));

            // Sailboat
            type = GetClassType(nameof(Sailboat));
            Assert.AreEqual(
                INestableTest.GetFieldCount(typeof(Sailboat)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Sailboat)));

            // Yacht
            type = GetClassType(nameof(Yacht));
            Assert.AreEqual(
                INestableTest.GetFieldCount(typeof(Yacht)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Yacht)));
        }

        [Test(Description = "Ensures the existance/absense of methods are handled correctly.")]
        public void MethodsExistTest()
        {
            // Boat
            Core.Models.INestable type = GetClassType(nameof(Data.Classes.Boat));
            Assert.AreEqual(
                INestableTest.GetMethodCount(typeof(Boat)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Boat)));

            // Canoe
            type = GetClassType(nameof(Canoe));
            Assert.AreEqual(
                INestableTest.GetMethodCount(typeof(Canoe)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Canoe)));

            // Runabout
            type = GetClassType(nameof(Runabout));
            Assert.AreEqual(
                INestableTest.GetMethodCount(typeof(Runabout)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Runabout)));

            // Sailboat
            type = GetClassType(nameof(Sailboat));
            Assert.AreEqual(
                INestableTest.GetMethodCount(typeof(Sailboat)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Sailboat)));

            // Yacht
            type = GetClassType(nameof(Yacht));
            Assert.AreEqual(
                INestableTest.GetMethodCount(typeof(Yacht)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Yacht)));
        }

        public INestable GetClassType(string className)
            => (Docs.ModelTree.Root
                .Namespaces["Docsharp"]
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Classes"]
                .Types[className] as TypeNode)
                .Member as INestable;
    }
}
