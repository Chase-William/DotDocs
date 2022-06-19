using Charp.Core.Models;
using Charp.Core.Models.Members;
using Charp.Test.Data.Classes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Charp.Core.Models.Types;

namespace Charp.Test.Members
{
    internal class MethodTest : BaseTest
    {
        [Test(Description = "Ensures the <IsPublic> member of the <MethodTest> type is set correctly.")]
        public void IsPublicSetCorrectly()
        {
            Assert.IsTrue(GetMethodModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.TryDock)).IsPublic);
            Assert.IsTrue(GetMethodModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.TryUndock)).IsPublic);
        }

        [Test(Description = "Ensures the <IsAbstract> member of the <MethodTest> type is set correctly.")]
        public void IsAbstractSetCorrectly()
        {
            Assert.IsTrue(GetMethodModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.TryDock)).IsAbstract);
            Assert.IsTrue(GetMethodModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.TryUndock)).IsAbstract);
        }

        [Test(Description = "Ensures the <IsVirtual> member of the <MethodTest> type is set correctly.")]
        public void IsVirtualSetCorrectly()
        {
            Assert.IsFalse(GetMethodModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.TryDock)).IsVirtual);
            Assert.IsFalse(GetMethodModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.TryUndock)).IsVirtual);
        }

        [Test(Description = "Ensures the <IsStatic> member of the <MethodTest> type is set correctly.")]
        public void IsStaticSetCorrectly()
        {
            Assert.IsFalse(GetMethodModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.TryDock)).IsStatic);
            Assert.IsFalse(GetMethodModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.TryUndock)).IsStatic);
            Assert.IsTrue(GetMethodModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.ExampleStaticMethod)).IsStatic);
        }

        [Test(Description = "Ensures the <ReturnType> member of the <MethodTest> type is set correctly.")]
        public void IsReturnTypeSetCorrectly()
        {
            Assert.AreEqual(typeof(bool).ToString(), GetMethodModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.TryDock)).ReturnType);
            Assert.AreEqual(typeof(bool).ToString(), GetMethodModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.TryUndock)).ReturnType);
            Assert.AreEqual(typeof(bool).ToString(), GetMethodModel(nameof(Data.Classes), nameof(Sailboat), nameof(Sailboat.TryDeploySailToPercentage)).ReturnType);
            Assert.AreEqual(typeof(void).ToString(), GetMethodModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.ExampleStaticMethod)).ReturnType);
        }

        [Test(Description = "Ensures the <Parameters> member of the <MethodTest> type is set correctly.")]
        public void IsParametersSetCorrectly()
        {
            Assert.IsEmpty(GetMethodModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.TryDock)).Parameters);
            Assert.IsEmpty(GetMethodModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.TryUndock)).Parameters);
            Assert.AreEqual(new Parameter
            {
                Name = "percentage",
                Type = typeof(double).ToString()
            },
            GetMethodModel(nameof(Data.Classes), nameof(Sailboat), nameof(Sailboat.TryDeploySailToPercentage))
            .Parameters[0]);
            Assert.AreEqual(new Parameter[]
            {
                new Parameter
                {
                    Name = "a",
                    Type = typeof(int).ToString()
                },
                new Parameter
                {
                    Name = "b",
                    Type = typeof(Sailboat).ToString()
                }
            },            
            GetMethodModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.ExampleStaticMethod))
            .Parameters);
        }

        [Test(Description = "Ensures the <IsProtected> member of the <EventModel> type is set correctly.")]
        public void IsProtectedSetCorrectly()
        {
            Assert.IsTrue(GetBoatClassMethod("ProtectedMethod").IsProtected);
            Assert.IsFalse(GetBoatClassMethod("InternalMethod").IsProtected);
            Assert.IsTrue(GetBoatClassMethod("InternalProtectedMethod").IsProtected);
            Assert.IsTrue(GetBoatClassMethod("StaticInternalProtectedMethod").IsProtected);
        }

        [Test(Description = "Ensures the <IsInternal> member of the <EventModel> type is set correctly.")]
        public void IsInternalSetCorrectly()
        {
            Assert.IsFalse(GetBoatClassMethod("ProtectedMethod").IsInternal);
            Assert.IsTrue(GetBoatClassMethod("InternalMethod").IsInternal);
            Assert.IsTrue(GetBoatClassMethod("InternalProtectedMethod").IsInternal);
            Assert.IsTrue(GetBoatClassMethod("StaticInternalProtectedMethod").IsInternal);
        }

        MethodModel GetBoatClassMethod(string methodName)
            => GetMethodModel(nameof(Data.Classes), nameof(Boat), methodName);        

        public MethodModel GetMethodModel(string _namespace, string className, string methodName)
            => (Docs.Models.Root
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces[_namespace]
                .Types[className]
                .Member as IMemberContainable)
            .Methods
            .Single(e => e.Name == methodName);
    }
}
