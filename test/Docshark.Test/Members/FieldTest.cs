using Docshark.Core.Models;
using Docshark.Core.Models.Members;
using Docshark.Core.Models.Types;
using Docshark.Test.Data.Classes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Test.Members
{
    internal class FieldTest : BaseTest
    {
        [Test(Description = "Ensures the <IsPublic> member of the <FieldTest> type is set correctly.")]
        public void IsPublicSetCorrectly()
        {
            Assert.AreEqual(true, GetField(nameof(Boat), nameof(Boat.MyField)).IsPublic);
            Assert.AreEqual(true, GetField(nameof(Boat), nameof(Boat.MyReadonlyField)).IsPublic);
            Assert.AreEqual(true, GetField(nameof(Boat), nameof(Boat.MyStaticField)).IsPublic);
            Assert.AreEqual(true, GetField(nameof(Boat), nameof(Boat.MyConstantField)).IsPublic);
            // Not currently exposing private fields
        }

        [Test(Description = "Ensures the <IsReadonly> member of the <FieldTest> type is set correctly.")]
        public void IsReadonlySetCorrectly()
        {
            Assert.AreEqual(false, GetField(nameof(Boat), nameof(Boat.MyField)).IsReadonly);
            Assert.AreEqual(true, GetField(nameof(Boat), nameof(Boat.MyReadonlyField)).IsReadonly);
            Assert.AreEqual(false, GetField(nameof(Boat), nameof(Boat.MyStaticField)).IsReadonly);
            Assert.AreEqual(false, GetField(nameof(Boat), nameof(Boat.MyConstantField)).IsReadonly);
            // Not currently exposing private fields
        }

        [Test(Description = "Ensures the <IsConstant> member of the <FieldTest> type is set correctly.")]
        public void IsConstantSetCorrectly()
        {
            Assert.AreEqual(false, GetField(nameof(Boat), nameof(Boat.MyField)).IsConstant);
            Assert.AreEqual(false, GetField(nameof(Boat), nameof(Boat.MyReadonlyField)).IsConstant);
            Assert.AreEqual(false, GetField(nameof(Boat), nameof(Boat.MyStaticField)).IsConstant);
            Assert.AreEqual(true, GetField(nameof(Boat), nameof(Boat.MyConstantField)).IsConstant);
            // Not currently exposing private fields
        }

        [Test(Description = "Ensures the <IsStatic> member of the <FieldTest> type is set correctly.")]
        public void IsStaticSetCorrectly()
        {
            Assert.AreEqual(false, GetField(nameof(Boat), nameof(Boat.MyField)).IsStatic);
            Assert.AreEqual(false, GetField(nameof(Boat), nameof(Boat.MyReadonlyField)).IsStatic);
            Assert.AreEqual(true, GetField(nameof(Boat), nameof(Boat.MyStaticField)).IsStatic);
            Assert.AreEqual(true, GetField(nameof(Boat), nameof(Boat.MyConstantField)).IsStatic);
            // Not currently exposing private fields
        }

        [Test(Description = "Ensures the <IsProtected> member of the <FieldTest> type is set correctly.")]
        public void IsProtectedSetCorrectly()
        {
            Assert.IsTrue(GetBoatClassField("ProtectedField").IsProtected);
            Assert.IsFalse(GetBoatClassField("InternalField").IsProtected);
            Assert.IsTrue(GetBoatClassField("InternalProtectedField").IsProtected);
            Assert.IsTrue(GetBoatClassField("StaticInternalProtectedField").IsProtected);
        }

        [Test(Description = "Ensures the <IsInternal> member of the <FieldTest> type is set correctly.")]
        public void IsInternalSetCorrectly()
        {
            Assert.IsFalse(GetBoatClassField("ProtectedField").IsInternal);
            Assert.IsTrue(GetBoatClassField("InternalField").IsInternal);
            Assert.IsTrue(GetBoatClassField("InternalProtectedField").IsInternal);
            Assert.IsTrue(GetBoatClassField("StaticInternalProtectedField").IsInternal);
        }

        FieldModel GetBoatClassField(string field)
            => GetField(nameof(Boat), field);

        public FieldModel GetField(string className, string fieldName)
            => (Docs.Models.Root
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Classes"]
                .Types[className]
                .Member as IMemberContainable)
            .Fields
            .Single(e => e.Name == fieldName);
    }
}
