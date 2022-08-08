using Docshark.Test.Data.Classes;
using NUnit.Framework;
using System.Linq;
using Docshark.Core.Models.Codebase.Members;
using Docshark.Core.Models.Codebase.Types;

namespace Docshark.Test.Members
{
    internal class EventTest : BaseTest
    {
        [Test(Description = "Ensures the <IsPublic> member of the <EventModel> type is set correctly.")]
        public void IsPublicSetCorrectly()
        {
            Assert.AreEqual(true, GetEvent(nameof(Boat), nameof(Boat.Docked)).IsPublic);
            Assert.AreEqual(true, GetEvent(nameof(Boat), nameof(Boat.PublicEvent)).IsPublic);
            Assert.AreEqual(true, GetEvent(nameof(Boat), nameof(Boat.StaticEvent)).IsPublic);
            Assert.AreEqual(true, GetEvent(nameof(Boat), nameof(Boat.UnDocked)).IsPublic);
            Assert.AreEqual(true, GetEvent(nameof(Boat), nameof(Boat.VirtualEvent)).IsPublic);
            // No private events are currently exposed
        }

        [Test(Description = "Ensures the <IsVirtual> member of the <EventModel> type is set correctly.")]
        public void IsVirtualSetCorrectly()
        {
            Assert.AreEqual(false, GetEvent(nameof(Boat), nameof(Boat.Docked)).IsVirtual);
            Assert.AreEqual(false, GetEvent(nameof(Boat), nameof(Boat.PublicEvent)).IsVirtual);
            Assert.AreEqual(false, GetEvent(nameof(Boat), nameof(Boat.StaticEvent)).IsVirtual);
            Assert.AreEqual(true, GetEvent(nameof(Boat), nameof(Boat.UnDocked)).IsVirtual);
            Assert.AreEqual(true, GetEvent(nameof(Boat), nameof(Boat.VirtualEvent)).IsVirtual);
            // Abstract events are also virtual behind the scenes it seems, but to make the docs
            // less confusing; we'll only except one or the other...
        }

        [Test(Description = "Ensures the <IsAbstract> member of the <EventModel> type is set correctly.")]
        public void IsAbstractSetCorrectly()
        {
            Assert.AreEqual(true, GetEvent(nameof(Boat), nameof(Boat.Docked)).IsAbstract);
            Assert.AreEqual(false, GetEvent(nameof(Boat), nameof(Boat.PublicEvent)).IsAbstract);
            Assert.AreEqual(false, GetEvent(nameof(Boat), nameof(Boat.StaticEvent)).IsAbstract);
            Assert.AreEqual(false, GetEvent(nameof(Boat), nameof(Boat.UnDocked)).IsAbstract);
            Assert.AreEqual(false, GetEvent(nameof(Boat), nameof(Boat.VirtualEvent)).IsAbstract);
        }

        [Test(Description = "Ensures the <IsAbstract> member of the <EventModel> type is set correctly.")]
        public void IsStaticSetCorrectly()
        {
            Assert.AreEqual(false, GetEvent(nameof(Boat), nameof(Boat.Docked)).IsStatic);
            Assert.AreEqual(false, GetEvent(nameof(Boat), nameof(Boat.PublicEvent)).IsStatic);
            Assert.AreEqual(true, GetEvent(nameof(Boat), nameof(Boat.StaticEvent)).IsStatic);
            Assert.AreEqual(false, GetEvent(nameof(Boat), nameof(Boat.UnDocked)).IsStatic);
            Assert.AreEqual(false, GetEvent(nameof(Boat), nameof(Boat.VirtualEvent)).IsStatic);
        }

        [Test(Description = "Ensures the <IsProtected> member of the <EventModel> type is set correctly.")]
        public void IsProtectedSetCorrectly()
        {
            Assert.IsTrue(GetBoatClassEvent("ProtectedEvent").IsProtected);
            Assert.IsFalse(GetBoatClassEvent("InternalEvent").IsProtected);
            Assert.IsTrue(GetBoatClassEvent("InternalProtectedEvent").IsProtected);
            Assert.IsTrue(GetBoatClassEvent("StaticInternalProtectedEvent").IsProtected);
        }

        [Test(Description = "Ensures the <IsInternal> member of the <EventModel> type is set correctly.")]
        public void IsInternalSetCorrectly()
        {
            Assert.IsFalse(GetBoatClassEvent("ProtectedEvent").IsInternal);
            Assert.IsTrue(GetBoatClassEvent("InternalEvent").IsInternal);
            Assert.IsTrue(GetBoatClassEvent("InternalProtectedEvent").IsInternal);
            Assert.IsTrue(GetBoatClassEvent("StaticInternalProtectedEvent").IsInternal);
        }

        EventModel GetBoatClassEvent(string eventName)
            => GetEvent(nameof(Boat), eventName);

        public EventModel GetEvent(string className, string eventName)
            => (Docs.Builder.ProjectManager.RootProject.Codebase.Root
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces["Classes"]
                .Types[className]
                .Member as IMemberContainable)
            .Events
            .Single(e => e.Name == eventName);
    }
}
