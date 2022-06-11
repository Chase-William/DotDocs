
using NUnit.Framework;

using Charp.Core.Tree;
using Charp.Core.Models;
using Charp.Test.Data.Classes;
using Charp.Test.Interfaces.Meta;
using Charp.Core.Models.Types;
using System;

namespace Charp.Test.Types
{
    internal class ClassTest : BaseTest, INestableTest
    {
        [Test(Description = "Ensures all class metadata in .dll exist.")]
        public void ClassExist()
        {
            // Boat
            Assert.NotNull(GetType<ClassModel>(nameof(Boat)));
            // Canoe
            Assert.NotNull(GetType<ClassModel>(nameof(Canoe)));
            // Runabout
            Assert.NotNull(GetType<ClassModel>(nameof(Runabout)));
            // Sailboat
            Assert.NotNull(GetType<ClassModel>(nameof(Sailboat)));
            // Yacht
            Assert.NotNull(GetType<ClassModel>(nameof(Yacht)));
        }

        [Test(Description = "Ensures the existance/absense of properties are handled correctly.")]
        public void PropertiesExistTest()
        {
            // Boat
            INestable type = GetType<ClassModel>(nameof(Boat));
            Assert.AreEqual(
                INestableTest.GetPropertyCount(typeof(Boat)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Boat)));

            // Canoe
            type = GetType<ClassModel>(nameof(Canoe));
            Assert.AreEqual(
                INestableTest.GetPropertyCount(typeof(Canoe)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Canoe)));

            // Runabout
            type = GetType<ClassModel>(nameof(Runabout));
            Assert.AreEqual(
                INestableTest.GetPropertyCount(typeof(Runabout)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Runabout)));

            // Sailboat
            type = GetType<ClassModel>(nameof(Sailboat));
            Assert.AreEqual(
                INestableTest.GetPropertyCount(typeof(Sailboat)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Sailboat)));

            // Yacht
            type = GetType<ClassModel>(nameof(Yacht));
            Assert.AreEqual(
                INestableTest.GetPropertyCount(typeof(Yacht)),
                type.Properties.Length,
                GetTypeTestMessage(typeof(Yacht)));
        }

        [Test(Description = "Ensures all public fields of fields are accounted for.")]
        public void FieldsExistTest()
        {
            // Boat
            INestable type = GetType<ClassModel>(nameof(Boat));
            Assert.AreEqual(
                INestableTest.GetFieldCount(typeof(Boat)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Boat)));

            // Canoe
            type = GetType<ClassModel>(nameof(Canoe));
            Assert.AreEqual(
                INestableTest.GetFieldCount(typeof(Canoe)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Canoe)));

            // Runabout
            type = GetType<ClassModel>(nameof(Runabout));
            Assert.AreEqual(
                INestableTest.GetFieldCount(typeof(Runabout)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Runabout)));

            // Sailboat
            type = GetType<ClassModel>(nameof(Sailboat));
            Assert.AreEqual(
                INestableTest.GetFieldCount(typeof(Sailboat)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Sailboat)));

            // Yacht
            type = GetType<ClassModel>(nameof(Yacht));
            Assert.AreEqual(
                INestableTest.GetFieldCount(typeof(Yacht)),
                type.Fields.Length,
                GetTypeTestMessage(typeof(Yacht)));
        }

        [Test(Description = "Ensures the existance/absense of methods are handled correctly.")]
        public void MethodsExistTest()
        {
            // Boat
            INestable type = GetType<ClassModel>(nameof(Boat));
            Assert.AreEqual(
                INestableTest.GetMethodCount(typeof(Boat)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Boat)));

            // Canoe
            type = GetType<ClassModel>(nameof(Canoe));
            Assert.AreEqual(
                INestableTest.GetMethodCount(typeof(Canoe)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Canoe)));

            // Runabout
            type = GetType<ClassModel>(nameof(Runabout));
            Assert.AreEqual(
                INestableTest.GetMethodCount(typeof(Runabout)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Runabout)));

            // Sailboat
            type = GetType<ClassModel>(nameof(Sailboat));
            Assert.AreEqual(
                INestableTest.GetMethodCount(typeof(Sailboat)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Sailboat)));

            // Yacht
            type = GetType<ClassModel>(nameof(Yacht));
            Assert.AreEqual(
                INestableTest.GetMethodCount(typeof(Yacht)),
                type.Methods.Length,
                GetTypeTestMessage(typeof(Yacht)));
        }

        [Test(Description = "Ensures the <IsPublic> member of the <ClassModel> type is set correctly.")]
        public void IsPublicSetCorrectly()
        {
            Assert.IsTrue(GetType<ClassModel>(nameof(Boat)).IsPublic);
            Assert.IsTrue(GetType<ClassModel>(nameof(Sailboat)).IsPublic);
            // Can't test true case... where is my <friend> modifier from C++?
        }

        [Test(Description = "Ensures the <IsAbstract> member of the <ClassModel> type is set correctly.")]
        public void IsAbstractSetCorrectly()
        {
            Assert.IsTrue(GetType<ClassModel>(nameof(Boat)).IsAbstract);
            Assert.IsFalse(GetType<ClassModel>(nameof(Sailboat)).IsAbstract);
        }

        [Test(Description = "Ensures the <IsInternal> member of the <ClassModel> type is set correctly.")]
        public void IsInternalSetCorrectly()
        {
            Assert.IsTrue(GetType<ClassModel>(nameof(Boat)).IsAbstract);
            Assert.IsFalse(GetType<ClassModel>(nameof(Sailboat)).IsAbstract);
            // Can't test true case... where is my <friend> modifier from C++?
        }

        [Test(Description = "Ensures the <IsSealed> member of the <ClassModel> type is set correctly.")]
        public void IsSealedSetCorrectly()
        {
            Assert.IsFalse(GetType<ClassModel>(nameof(Boat)).IsSealed);
            Assert.IsTrue(GetType<ClassModel>(nameof(Sailboat)).IsSealed);
        }

        [Test(Description = "Ensures the <IsStatic> member of the <ClassModel> type is set correctly.")]
        public void IsStaticSetCorrectly()
        {
            Assert.IsFalse(GetType<ClassModel>(nameof(Boat)).IsStatic);
            Assert.IsFalse(GetType<ClassModel>(nameof(Sailboat)).IsStatic);
            Assert.IsTrue(GetType<ClassModel>(nameof(BoatUtil)).IsStatic);
        }

        [Test(Description = "Ensures the <Parent> member of the <ClassModel> type is set correctly.")]
        public void IsParentSetCorrectly()
        {
            Assert.AreEqual(typeof(object).ToString(), GetType<ClassModel>(nameof(Boat)).Parent);
            Assert.AreEqual(typeof(Boat).ToString(), GetType<ClassModel>(nameof(Sailboat)).Parent);
            Assert.AreEqual(typeof(object).ToString(), GetType<ClassModel>(nameof(BoatUtil)).Parent);
        }

        //public ClassModel GetType(string className)
        //    => Docs.Models.Root
        //        .Namespaces["Test"]
        //        .Namespaces["Data"]
        //        .Namespaces["Classes"]
        //        .Types[className]
        //        .Member as ClassModel;

        public T GetType<T>(string className) where T : INestable
            => (T)(Docs.Models.Root
                    .Namespaces["Test"]
                    .Namespaces["Data"]
                    .Namespaces["Classes"]
                    .Types[className]
                    .Member as INestable);
    }
}
