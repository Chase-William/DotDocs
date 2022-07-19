using Docshark.Core.Models.Types;
using Docshark.Core.Tree;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Test.Types
{
    internal class TypeTest : BaseTest
    {
        [Test(Description = "Ensures the <IsPublic> member of the <TypeModel> type is set correctly.")]
        public void IsPublicSetCorrectly()
        {
            // Public Class & Nested
            Assert.IsTrue(ClassTest.GetClassModel("PublicClass").IsPublic);
            Assert.IsTrue(GetNestedPublicClassClassModel("NestedPublicClass").IsPublic);
            Assert.IsFalse(GetNestedPublicClassClassModel("NestedPrivateClass").IsPublic);
            Assert.IsFalse(GetNestedPublicClassClassModel("NestedProtectedClass").IsPublic);
            Assert.IsFalse(GetNestedPublicClassClassModel("NestedInternalClass").IsPublic);
            Assert.IsFalse(GetNestedPublicClassClassModel("NestedInternalProtectedClass").IsPublic);

            // Internal Class & Nested
            Assert.IsFalse(ClassTest.GetClassModel("InternalClass").IsPublic);
            Assert.IsTrue(GetNestedPrivateClassClassModel("NestedPublicClass").IsPublic);
            Assert.IsFalse(GetNestedPrivateClassClassModel("NestedPrivateClass").IsPublic);
            Assert.IsFalse(GetNestedPublicClassClassModel("NestedProtectedClass").IsPublic);
            Assert.IsFalse(GetNestedPrivateClassClassModel("NestedInternalClass").IsPublic);
            Assert.IsFalse(GetNestedPrivateClassClassModel("NestedInternalProtectedClass").IsPublic);
        }

        [Test(Description = "Ensures the <IsPrivate> member of the <TypeModel> type is set correctly.")]
        public void IsPrivateSetCorrectly()
        {
            // Public Class & Nested
            Assert.IsFalse(ClassTest.GetClassModel("PublicClass").IsPrivate);
            Assert.IsFalse(GetNestedPublicClassClassModel("NestedPublicClass").IsPrivate);
            Assert.IsTrue(GetNestedPublicClassClassModel("NestedPrivateClass").IsPrivate);
            Assert.IsFalse(GetNestedPublicClassClassModel("NestedProtectedClass").IsPrivate);
            Assert.IsFalse(GetNestedPublicClassClassModel("NestedInternalClass").IsPrivate);
            Assert.IsFalse(GetNestedPublicClassClassModel("NestedInternalProtectedClass").IsPrivate);

            // Internal Class & Nested
            Assert.IsFalse(ClassTest.GetClassModel("InternalClass").IsPrivate);
            Assert.IsFalse(GetNestedPrivateClassClassModel("NestedPublicClass").IsPrivate);
            Assert.IsTrue(GetNestedPrivateClassClassModel("NestedPrivateClass").IsPrivate);
            Assert.IsFalse(GetNestedPublicClassClassModel("NestedProtectedClass").IsPrivate);
            Assert.IsFalse(GetNestedPrivateClassClassModel("NestedInternalClass").IsPrivate);
            Assert.IsFalse(GetNestedPrivateClassClassModel("NestedInternalProtectedClass").IsPrivate);
        }

        [Test(Description = "Ensures the <IsInternal> member of the <TypeModel> type is set correctly.")]
        public void IsInternalSetCorrectly()
        {
            // Public Class & Nested
            Assert.IsFalse(ClassTest.GetClassModel("PublicClass").IsInternal);
            Assert.IsFalse(GetNestedPublicClassClassModel("NestedPublicClass").IsInternal);
            Assert.IsFalse(GetNestedPublicClassClassModel("NestedPrivateClass").IsInternal);
            Assert.IsFalse(GetNestedPublicClassClassModel("NestedProtectedClass").IsInternal);
            Assert.IsTrue(GetNestedPublicClassClassModel("NestedInternalClass").IsInternal);
            Assert.IsTrue(GetNestedPublicClassClassModel("NestedInternalProtectedClass").IsInternal);

            // Internal Class & Nested
            Assert.IsTrue(ClassTest.GetClassModel("InternalClass").IsInternal);
            Assert.IsFalse(GetNestedPrivateClassClassModel("NestedPublicClass").IsInternal);
            Assert.IsFalse(GetNestedPrivateClassClassModel("NestedPrivateClass").IsInternal);
            Assert.IsFalse(GetNestedPublicClassClassModel("NestedProtectedClass").IsInternal);
            Assert.IsTrue(GetNestedPrivateClassClassModel("NestedInternalClass").IsInternal);
            Assert.IsTrue(GetNestedPrivateClassClassModel("NestedInternalProtectedClass").IsInternal);
        }

        [Test(Description = "Ensures the <IsProtected> member of the <TypeModel> type is set correctly.")]
        public void IsProtectedSetCorrectly()
        {
            // Public Class & Nested
            Assert.IsFalse(ClassTest.GetClassModel("PublicClass").IsProtected);
            Assert.IsFalse(GetNestedPublicClassClassModel("NestedPublicClass").IsProtected);
            Assert.IsFalse(GetNestedPublicClassClassModel("NestedPrivateClass").IsProtected);
            Assert.IsTrue(GetNestedPublicClassClassModel("NestedProtectedClass").IsProtected);
            Assert.IsFalse(GetNestedPublicClassClassModel("NestedInternalClass").IsProtected);
            Assert.IsTrue(GetNestedPublicClassClassModel("NestedInternalProtectedClass").IsProtected);

            // Internal Class & Nested
            Assert.IsFalse(ClassTest.GetClassModel("InternalClass").IsProtected);
            Assert.IsFalse(GetNestedPrivateClassClassModel("NestedPublicClass").IsProtected);
            Assert.IsFalse(GetNestedPrivateClassClassModel("NestedPrivateClass").IsProtected);
            Assert.IsTrue(GetNestedPublicClassClassModel("NestedProtectedClass").IsProtected);
            Assert.IsFalse(GetNestedPrivateClassClassModel("NestedInternalClass").IsProtected);
            Assert.IsTrue(GetNestedPrivateClassClassModel("NestedInternalProtectedClass").IsProtected);
        }

        public static ClassModel GetNestedPublicClassClassModel(string className)
            => (ClassModel)(Docs.Models.Root
                    .Namespaces["Test"]
                    .Namespaces["Data"]
                    .Namespaces["Classes"]
                    .Types["PublicClass"]
                    as TypeNodeNestable)
            .Types[className].Member;

        public static ClassModel GetNestedPrivateClassClassModel(string className)
            => (ClassModel)(Docs.Models.Root
                    .Namespaces["Test"]
                    .Namespaces["Data"]
                    .Namespaces["Classes"]
                    .Types["InternalClass"]
                    as TypeNodeNestable)
            .Types[className].Member;
    }
}
