using Docshark.Core.Models.Codebase.Types;
using Docshark.Test.Data.Structs;
using NUnit.Framework;
using System;

namespace Docshark.Test.Types
{
    internal class StructTest : BaseTest
    {
        [Test(Description = "Ensures the <IsPublic> member of the <StructModel> type is set correctly.")]
        public void IsPublicSetCorrectly()
        {
            Assert.IsTrue(GetStructModel(nameof(Point)).IsPublic);
            Assert.IsTrue(GetStructModel(nameof(Rectangle)).IsPublic);
            // Can't test private == true
        }

        [Test(Description = "Ensures the <IsInternal> member of the <StructModel> type is set correctly.")]
        public void IsInternalSetCorrectly()
        {
            Assert.IsFalse(GetStructModel(nameof(Point)).IsInternal);
            Assert.IsFalse(GetStructModel(nameof(Rectangle)).IsInternal);
            // Can't test internal == true
        }

        [Test(Description = "Ensures the <Parent> member of the <StructModel> type is set correctly.")]
        public void ParentSetCorrectly()
        {
            Assert.AreEqual(typeof(ValueType).ToString(), GetStructModel(nameof(Point)).Parent);
            Assert.AreEqual(typeof(ValueType).ToString(), GetStructModel(nameof(Rectangle)).Parent);
            // Can't test internal == true
        }

        public static StructModel GetStructModel(string structName)
            => Docs.Builder.ProjectManager.RootProject.Codebase.Root
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Structs"]
                .Types[structName]
                .Member as StructModel;
    }
}
