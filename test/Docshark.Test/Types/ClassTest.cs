
using NUnit.Framework;

using Docshark.Core.Tree;
using Docshark.Core.Models;
using Docshark.Test.Data.Classes;
using System;
using Docshark.Core.Models.Lang.Types;
using Docshark.Test.Interfaces.Meta;

namespace Docshark.Test.Types
{
    internal class ClassTest : BaseTest
    {
        [Test(Description = "Ensures all class metadata in .dll exist.")]
        public void ClassExist()
        {
            // Boat
            Assert.NotNull(GetClassModel(nameof(Boat)));
            // Canoe
            Assert.NotNull(GetClassModel(nameof(Canoe)));
            // Runabout
            Assert.NotNull(GetClassModel(nameof(Runabout)));
            // Sailboat
            Assert.NotNull(GetClassModel(nameof(Sailboat)));
            // Yacht
            Assert.NotNull(GetClassModel(nameof(Yacht)));
        }

        [Test(Description = "Ensures the existance/absense of properties are handled correctly.")]
        public void PropertiesExistTest()
        {
            // Boat
            IMemberContainable type = GetClassModel(nameof(Boat));
            Assert.AreEqual(
                INestableTest.GetPropertyCount(typeof(Boat)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Boat)));

            // Canoe
            type = GetClassModel(nameof(Canoe));
            Assert.AreEqual(
                INestableTest.GetPropertyCount(typeof(Canoe)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Canoe)));

            // Runabout
            type = GetClassModel(nameof(Runabout));
            Assert.AreEqual(
                INestableTest.GetPropertyCount(typeof(Runabout)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Runabout)));

            // Sailboat
            type = GetClassModel(nameof(Sailboat));
            Assert.AreEqual(
                INestableTest.GetPropertyCount(typeof(Sailboat)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Sailboat)));

            // Yacht
            type = GetClassModel(nameof(Yacht));
            Assert.AreEqual(
                INestableTest.GetPropertyCount(typeof(Yacht)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Yacht)));
        }

        [Test(Description = "Ensures all public fields of fields are accounted for.")]
        public void FieldsExistTest()
        {
            // Boat
            IMemberContainable type = GetClassModel(nameof(Boat));
            Assert.AreEqual(
                INestableTest.GetFieldCount(typeof(Boat)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Boat)));

            // Canoe
            type = GetClassModel(nameof(Canoe));
            Assert.AreEqual(
                INestableTest.GetFieldCount(typeof(Canoe)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Canoe)));

            // Runabout
            type = GetClassModel(nameof(Runabout));
            Assert.AreEqual(
                INestableTest.GetFieldCount(typeof(Runabout)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Runabout)));

            // Sailboat
            type = GetClassModel(nameof(Sailboat));
            Assert.AreEqual(
                INestableTest.GetFieldCount(typeof(Sailboat)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Sailboat)));

            // Yacht
            type = GetClassModel(nameof(Yacht));
            Assert.AreEqual(
                INestableTest.GetFieldCount(typeof(Yacht)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Yacht)));
        }

        [Test(Description = "Ensures the existance/absense of methods are handled correctly.")]
        public void MethodsExistTest()
        {
            // Boat
            IMemberContainable type = GetClassModel(nameof(Boat));
            Assert.AreEqual(
                INestableTest.GetMethodCount(typeof(Boat)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Boat)));

            // Canoe
            type = GetClassModel(nameof(Canoe));
            Assert.AreEqual(
                INestableTest.GetMethodCount(typeof(Canoe)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Canoe)));

            // Runabout
            type = GetClassModel(nameof(Runabout));
            Assert.AreEqual(
                INestableTest.GetMethodCount(typeof(Runabout)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Runabout)));

            // Sailboat
            type = GetClassModel(nameof(Sailboat));
            Assert.AreEqual(
                INestableTest.GetMethodCount(typeof(Sailboat)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Sailboat)));

            // Yacht
            type = GetClassModel(nameof(Yacht));
            Assert.AreEqual(
                INestableTest.GetMethodCount(typeof(Yacht)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Yacht)));
        }

        [Test(Description = "Ensures the <IsPublic> member of the <ClassModel> type is set correctly.")]
        public void IsPublicSetCorrectly()
        {
            Assert.IsTrue(GetClassModel(nameof(Boat)).IsPublic);
            Assert.IsTrue(GetClassModel(nameof(Sailboat)).IsPublic);
            // Can't test true case... where is my <friend> modifier from C++?
        }

        [Test(Description = "Ensures the <IsAbstract> member of the <ClassModel> type is set correctly.")]
        public void IsAbstractSetCorrectly()
        {
            Assert.IsTrue(GetClassModel(nameof(Boat)).IsAbstract);
            Assert.IsFalse(GetClassModel(nameof(Sailboat)).IsAbstract);
        }

        [Test(Description = "Ensures the <IsInternal> member of the <ClassModel> type is set correctly.")]
        public void IsInternalSetCorrectly()
        {
            Assert.IsTrue(GetClassModel(nameof(Boat)).IsAbstract);
            Assert.IsFalse(GetClassModel(nameof(Sailboat)).IsAbstract);
            // Can't test true case... where is my <friend> modifier from C++?
        }

        [Test(Description = "Ensures the <IsSealed> member of the <ClassModel> type is set correctly.")]
        public void IsSealedSetCorrectly()
        {
            Assert.IsFalse(GetClassModel(nameof(Boat)).IsSealed);
            Assert.IsTrue(GetClassModel(nameof(Sailboat)).IsSealed);
        }

        [Test(Description = "Ensures the <IsStatic> member of the <ClassModel> type is set correctly.")]
        public void IsStaticSetCorrectly()
        {
            Assert.IsFalse(GetClassModel(nameof(Boat)).IsStatic);
            Assert.IsFalse(GetClassModel(nameof(Sailboat)).IsStatic);
            Assert.IsTrue(GetClassModel(nameof(BoatUtil)).IsStatic);
        }

        [Test(Description = "Ensures the <Parent> member of the <ClassModel> type is set correctly.")]
        public void IsParentSetCorrectly()
        {
            Assert.AreEqual(typeof(object).ToString(), GetClassModel(nameof(Boat)).Parent);
            Assert.AreEqual(typeof(Boat).ToString(), GetClassModel(nameof(Sailboat)).Parent);
            Assert.AreEqual(typeof(object).ToString(), GetClassModel(nameof(BoatUtil)).Parent);
        }

        //public ClassModel GetType(string className)
        //    => Docs.Models.Root
        //        .Namespaces["Test"]
        //        .Namespaces["Data"]
        //        .Namespaces["Classes"]
        //        .Types[className]
        //        .Member as ClassModel;

        public static ClassModel GetClassModel(string className)
            => (ClassModel)(Docs.Builder.Models.Root
                    .Namespaces["Test"]
                    .Namespaces["Data"]
                    .Namespaces["Classes"]
                    .Types[className]
                    .Member);
    }
}
