using Docshark.Core.Mapper.Codebase.Nodes;
using Docshark.Core.Models.Codebase.Types;
using Docshark.Test.Data.Delegates;
using NUnit.Framework;
using System;

namespace Docshark.Test.Types
{
    internal class DelegateTest : BaseTest
    {
        [Test(Description = "Ensures the <IsPublic> member of the <DelegateModel> type is set correctly.")]
        public void IsPublicSetCorrectly()
        {
            Assert.IsTrue(GetDelegateModel(nameof(DoSomeMathWillYou)).IsPublic);
            Assert.IsTrue(GetNestedInClassDelegateModel(
                    nameof(NestedInClassDelegate),
                    nameof(NestedInClassDelegate.DoSomeNestedStuff))
                .IsPublic);
            // Can't test private case...
        }

        [Test(Description = "Ensures the <IsInternal> member of the <DelegateModel> type is set correctly.")]
        public void IsInternalSetCorrectly()
        {
            Assert.IsFalse(GetDelegateModel(nameof(DoSomeMathWillYou)).IsInternal);
            Assert.IsFalse(
                GetNestedInClassDelegateModel(
                    nameof(NestedInClassDelegate),
                    nameof(NestedInClassDelegate.DoSomeNestedStuff))
                .IsInternal);
            // Can't test private case...
        }

        [Test(Description = "Ensures the <Parent> member of the <DelegateModel> type is set correctly.")]
        public void ParentSetCorrectly()
        {
            Assert.AreEqual(
                typeof(MulticastDelegate).ToString(), 
                GetDelegateModel(nameof(DoSomeMathWillYou)).Parent);
            Assert.AreEqual(
                typeof(MulticastDelegate).ToString(), 
                GetNestedInClassDelegateModel(
                    nameof(NestedInClassDelegate), 
                    nameof(NestedInClassDelegate.DoSomeNestedStuff))
                .Parent);
        }

        public static DelegateModel GetDelegateModel(string delegateName)
            => Docs.Builder.ProjectManager.RootProject.Codebase.Root
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Delegates"]
                .Types[delegateName]
                .Member as DelegateModel;

        public static DelegateModel GetNestedInClassDelegateModel(string className, string delegateName)
         => (Docs.Builder.ProjectManager.RootProject.Codebase.Root
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Delegates"]
                .Types[className] as TypeNodeNestable)                
                .Types[delegateName]
            .Member as DelegateModel;
    }
}
