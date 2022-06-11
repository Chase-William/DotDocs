using Charp.Core.Models.Types;
using Charp.Test.Data.Structs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charp.Test.Types
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

        public StructModel GetStructModel(string structName)
            => Docs.Models.Root
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Structs"]
                .Types[structName]
                .Member as StructModel;
    }
}
