using Docshark.Core.Models;
using Docshark.Test.Data.Classes;
using NUnit.Framework;
using System.Linq;
using Docshark.Core.Models.Codebase.Members;
using Docshark.Core.Models.Codebase.Types;

namespace Docshark.Test.Members
{
    internal class PropertyTest : BaseTest
    {
        [Test(Description = "Ensures the <HasSetter> member of the <PropertyModel> type is set correctly.")]
        public void HasSetterSetCorrectly()
        {
            Assert.IsTrue(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Length)).HasSetter);
            Assert.IsTrue(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Weight)).HasSetter);
            Assert.IsFalse(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.NoSetterProperty)).HasSetter);
            // No private events are currently exposed
        }

        [Test(Description = "Ensures the <HasGetter> member of the <PropertyModel> type is set correctly.")]
        public void HasGetterSetCorrectly()
        {
            Assert.IsTrue(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Length)).HasGetter);
            Assert.IsTrue(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Weight)).HasGetter);
            Assert.IsFalse(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.NoGetterProperty)).HasGetter);
            // No private events are currently exposed
        }

        [Test(Description = "Ensures the <IsSetPublic> member of the <PropertyModel> type is set correctly.")]
        public void IsSetPublicSetCorrectly()
        {
            Assert.IsTrue(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Length)).IsSetPublic);
            Assert.IsTrue(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Weight)).IsSetPublic);
            Assert.IsFalse(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.PrivateSetterProperty)).IsSetPublic);
            // No private events are currently exposed
        }

        [Test(Description = "Ensures the <IsGetPublic> member of the <PropertyModel> type is set correctly.")]
        public void IsGetPublicSetCorrectly()
        {
            Assert.IsTrue(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Length)).IsGetPublic);
            Assert.IsTrue(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Weight)).IsGetPublic);
            Assert.IsFalse(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.PrivateGetterProperty)).IsGetPublic);
            // No private events are currently exposed
        }

        [Test(Description = "Ensures the <IsVirtual> member of the <PropertyModel> type is set correctly.")]
        public void IsVirtualSetCorrectly()
        {
            // Assert.IsNull(GetBoatClassProperty(nameof(Boat.Length)).Test);
            Assert.IsFalse(GetBoatClassProperty(nameof(Boat.Length)).IsVirtual);
            Assert.IsFalse(GetBoatClassProperty(nameof(Boat.Weight)).IsVirtual);
            Assert.IsFalse(GetBoatClassProperty("AbstractProperty").IsVirtual);
            Assert.IsFalse(GetBoatClassProperty("AbstractNoSetProperty").IsVirtual);
            Assert.IsFalse(GetBoatClassProperty("AbstractNoGetProperty").IsVirtual);
            Assert.IsTrue(GetBoatClassProperty("VirtualProperty").IsVirtual);
            Assert.IsTrue(GetBoatClassProperty("VirtualNoSetProperty").IsVirtual);
            Assert.IsTrue(GetBoatClassProperty("VirtualNoGetProperty").IsVirtual);
        }

        [Test(Description = "Ensures the <IsAbstract> member of the <PropertyModel> type is set correctly.")]
        public void IsAbstractSetCorrectly()
        {
            Assert.IsFalse(GetBoatClassProperty(nameof(Boat.Length)).IsAbstract);
            Assert.IsFalse(GetBoatClassProperty(nameof(Boat.Weight)).IsAbstract);
            Assert.IsTrue(GetBoatClassProperty("AbstractProperty").IsAbstract);
            Assert.IsTrue(GetBoatClassProperty("AbstractNoSetProperty").IsAbstract);
            Assert.IsTrue(GetBoatClassProperty("AbstractNoGetProperty").IsAbstract);
            Assert.IsFalse(GetBoatClassProperty("VirtualProperty").IsAbstract);
            Assert.IsFalse(GetBoatClassProperty("VirtualNoSetProperty").IsAbstract);
            Assert.IsFalse(GetBoatClassProperty("VirtualNoGetProperty").IsAbstract);
        }

        [Test(Description = "Ensures the <IsAbstract> member of the <PropertyModel> type is set correctly.")]
        public void IsStaticSetCorrectly()
        {
            Assert.IsFalse(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Length)).IsVirtual);
            Assert.IsFalse(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Weight)).IsVirtual);
        }

        [Test(Description = "Ensures the <IsGetProtected> member of the <PropertyModel> type is set correctly.")]
        public void IsGetProtected()
        {
            //Assert.AreEqual(null, GetBoatClassProperty("SetProtectedProperty").Test);
            // Standalone Protected
            Assert.IsTrue(GetBoatClassProperty("FullProtectedProperty").IsGetProtected);
            Assert.IsTrue(GetBoatClassProperty("GetProtectedProperty").IsGetProtected);
            Assert.IsFalse(GetBoatClassProperty("SetProtectedProperty").IsGetProtected);
            Assert.IsTrue(GetBoatClassProperty("GetProtectedNoSetProperty").IsGetProtected);
            Assert.IsNull(GetBoatClassProperty("GetProtectedNoGetProperty").IsGetProtected);

            // Standalone Internal
            Assert.IsFalse(GetBoatClassProperty("InternalProperty").IsGetProtected);
            Assert.IsFalse(GetBoatClassProperty("InternalGetProperty").IsGetProtected);
            Assert.IsFalse(GetBoatClassProperty("InternalSetProperty").IsGetProtected);
            Assert.IsFalse(GetBoatClassProperty("InternalNoSetProperty").IsGetProtected);
            Assert.IsNull(GetBoatClassProperty("InternalNoGetProperty").IsGetProtected);

            // Internal & Protected
            Assert.IsTrue(GetBoatClassProperty("InternalProtectedProperty").IsGetProtected);
            Assert.IsTrue(GetBoatClassProperty("InternalProtectedGetProperty").IsGetProtected);
            Assert.IsFalse(GetBoatClassProperty("InternalProtectedSetProperty").IsGetProtected);
            Assert.IsTrue(GetBoatClassProperty("InternalProtectedNoSetProperty").IsGetProtected);
            Assert.IsNull(GetBoatClassProperty("InternalProtectedNoGetProperty").IsGetProtected);

            // Static & Internal & Protected
            Assert.IsTrue(GetBoatClassProperty("StaticInternalProtectedProperty").IsGetProtected);
            Assert.IsNull(GetBoatClassProperty("StaticInternalProtectedNoGetProperty").IsGetProtected);
            Assert.IsTrue(GetBoatClassProperty("StaticInternalProtectedNoSetProperty").IsGetProtected);
        }

        [Test(Description = "Ensures the <IsSetProtected> member of the <PropertyModel> type is set correctly.")]
        public void IsSetProtected()
        {
            // Standalone Protected
            Assert.IsTrue(GetBoatClassProperty("FullProtectedProperty").IsSetProtected);
            Assert.IsFalse(GetBoatClassProperty("GetProtectedProperty").IsSetProtected);
            Assert.IsTrue(GetBoatClassProperty("SetProtectedProperty").IsSetProtected);
            Assert.IsNull(GetBoatClassProperty("GetProtectedNoSetProperty").IsSetProtected);
            Assert.IsTrue(GetBoatClassProperty("GetProtectedNoGetProperty").IsSetProtected);

            // Standalone Internal
            Assert.IsFalse(GetBoatClassProperty("InternalProperty").IsSetProtected);
            Assert.IsFalse(GetBoatClassProperty("InternalGetProperty").IsSetProtected);
            Assert.IsFalse(GetBoatClassProperty("InternalSetProperty").IsSetProtected);
            Assert.IsNull(GetBoatClassProperty("InternalNoSetProperty").IsSetProtected);
            Assert.IsFalse(GetBoatClassProperty("InternalNoGetProperty").IsSetProtected);

            // Internal & Protected
            Assert.IsTrue(GetBoatClassProperty("InternalProtectedProperty").IsSetProtected);
            Assert.IsFalse(GetBoatClassProperty("InternalProtectedGetProperty").IsSetProtected);
            Assert.IsTrue(GetBoatClassProperty("InternalProtectedSetProperty").IsSetProtected);
            Assert.IsNull(GetBoatClassProperty("InternalProtectedNoSetProperty").IsSetProtected);
            Assert.IsTrue(GetBoatClassProperty("InternalProtectedNoGetProperty").IsSetProtected);

            // Static & Internal & Protected
            Assert.IsTrue(GetBoatClassProperty("StaticInternalProtectedProperty").IsSetProtected);
            Assert.IsTrue(GetBoatClassProperty("StaticInternalProtectedNoGetProperty").IsSetProtected);
            Assert.IsNull(GetBoatClassProperty("StaticInternalProtectedNoSetProperty").IsSetProtected);
        }

        [Test(Description = "Ensures the <IsGetInternal> member of the <PropertyModel> type is set correctly.")]
        public void IsGetInternal()
        {
            // Standalone Protected
            Assert.IsFalse(GetBoatClassProperty("FullProtectedProperty").IsGetInternal);
            Assert.IsFalse(GetBoatClassProperty("GetProtectedProperty").IsGetInternal);
            Assert.IsFalse(GetBoatClassProperty("SetProtectedProperty").IsGetInternal);
            Assert.IsFalse(GetBoatClassProperty("GetProtectedNoSetProperty").IsGetInternal);
            Assert.IsNull(GetBoatClassProperty("GetProtectedNoGetProperty").IsGetInternal);

            // Standalone Internal
            Assert.IsTrue(GetBoatClassProperty("InternalProperty").IsGetInternal);
            Assert.IsTrue(GetBoatClassProperty("InternalGetProperty").IsGetInternal);
            Assert.IsFalse(GetBoatClassProperty("InternalSetProperty").IsGetInternal);
            Assert.IsTrue(GetBoatClassProperty("InternalNoSetProperty").IsGetInternal);
            Assert.IsNull(GetBoatClassProperty("InternalNoGetProperty").IsGetInternal);

            // Internal & Protected
            Assert.IsTrue(GetBoatClassProperty("InternalProtectedProperty").IsGetInternal);
            Assert.IsTrue(GetBoatClassProperty("InternalProtectedGetProperty").IsGetInternal);
            Assert.IsFalse(GetBoatClassProperty("InternalProtectedSetProperty").IsGetInternal);
            Assert.IsTrue(GetBoatClassProperty("InternalProtectedNoSetProperty").IsGetInternal);
            Assert.IsNull(GetBoatClassProperty("InternalProtectedNoGetProperty").IsGetInternal);

            // Static & Internal & Protected
            Assert.IsTrue(GetBoatClassProperty("StaticInternalProtectedProperty").IsGetInternal);
            Assert.IsNull(GetBoatClassProperty("StaticInternalProtectedNoGetProperty").IsGetInternal);
            Assert.IsTrue(GetBoatClassProperty("StaticInternalProtectedNoSetProperty").IsGetInternal);
        }

        [Test(Description = "Ensures the <IsSetInternal> member of the <PropertyModel> type is set correctly.")]
        public void IsSetInternal()
        {
            // Standalone Protected
            Assert.IsFalse(GetBoatClassProperty("FullProtectedProperty").IsSetInternal);
            Assert.IsFalse(GetBoatClassProperty("GetProtectedProperty").IsSetInternal);
            Assert.IsFalse(GetBoatClassProperty("SetProtectedProperty").IsSetInternal);
            Assert.IsNull(GetBoatClassProperty("GetProtectedNoSetProperty").IsSetInternal);
            Assert.IsFalse(GetBoatClassProperty("GetProtectedNoGetProperty").IsSetInternal);

            // Standalone Internal
            Assert.IsTrue(GetBoatClassProperty("InternalProperty").IsSetInternal);
            Assert.IsFalse(GetBoatClassProperty("InternalGetProperty").IsSetInternal);
            Assert.IsTrue(GetBoatClassProperty("InternalSetProperty").IsSetInternal);
            Assert.IsNull(GetBoatClassProperty("InternalNoSetProperty").IsSetInternal);
            Assert.IsTrue(GetBoatClassProperty("InternalNoGetProperty").IsSetInternal);

            // Internal & Protected
            Assert.IsTrue(GetBoatClassProperty("InternalProtectedProperty").IsSetInternal);
            Assert.IsFalse(GetBoatClassProperty("InternalProtectedGetProperty").IsSetInternal);
            Assert.IsTrue(GetBoatClassProperty("InternalProtectedSetProperty").IsSetInternal);
            Assert.IsNull(GetBoatClassProperty("InternalProtectedNoSetProperty").IsSetInternal);
            Assert.IsTrue(GetBoatClassProperty("InternalProtectedNoGetProperty").IsSetInternal);

            // Static & Internal & Protected
            Assert.IsTrue(GetBoatClassProperty("StaticInternalProtectedProperty").IsSetInternal);
            Assert.IsTrue(GetBoatClassProperty("StaticInternalProtectedNoGetProperty").IsSetInternal);
            Assert.IsNull(GetBoatClassProperty("StaticInternalProtectedNoSetProperty").IsSetInternal);
        }

        [Test(Description = "Ensures the <IsProtected> member of the <PropertyModel> type is set correctly.")]
        public void IsProtected()
        {
            // Standalone Protected
            Assert.IsTrue(GetBoatClassProperty("FullProtectedProperty").IsProtected);
            Assert.IsFalse(GetBoatClassProperty("GetProtectedProperty").IsProtected);
            Assert.IsFalse(GetBoatClassProperty("SetProtectedProperty").IsProtected);
            Assert.IsTrue(GetBoatClassProperty("GetProtectedNoSetProperty").IsProtected);
            Assert.IsTrue(GetBoatClassProperty("GetProtectedNoGetProperty").IsProtected);

            // Standalone Internal
            Assert.IsFalse(GetBoatClassProperty("InternalProperty").IsProtected);
            Assert.IsFalse(GetBoatClassProperty("InternalGetProperty").IsProtected);
            Assert.IsFalse(GetBoatClassProperty("InternalSetProperty").IsProtected);
            Assert.IsFalse(GetBoatClassProperty("InternalNoSetProperty").IsProtected);
            Assert.IsFalse(GetBoatClassProperty("InternalNoGetProperty").IsProtected);

            // Internal & Protected
            Assert.IsTrue(GetBoatClassProperty("InternalProtectedProperty").IsProtected);
            Assert.IsFalse(GetBoatClassProperty("InternalProtectedGetProperty").IsProtected);
            Assert.IsFalse(GetBoatClassProperty("InternalProtectedSetProperty").IsProtected);
            Assert.IsTrue(GetBoatClassProperty("InternalProtectedNoSetProperty").IsProtected);
            Assert.IsTrue(GetBoatClassProperty("InternalProtectedNoGetProperty").IsProtected);

            // Static & Internal & Protected
            Assert.IsTrue(GetBoatClassProperty("StaticInternalProtectedProperty").IsProtected);
            Assert.IsTrue(GetBoatClassProperty("StaticInternalProtectedNoGetProperty").IsProtected);
            Assert.IsTrue(GetBoatClassProperty("StaticInternalProtectedNoSetProperty").IsProtected);
        }

        [Test(Description = "Ensures the <IsInternal> member of the <PropertyModel> type is set correctly.")]
        public void IsInternal()
        {
            // Standalone Protected
            Assert.IsFalse(GetBoatClassProperty("FullProtectedProperty").IsInternal);
            Assert.IsFalse(GetBoatClassProperty("GetProtectedProperty").IsInternal);
            Assert.IsFalse(GetBoatClassProperty("SetProtectedProperty").IsInternal);
            Assert.IsFalse(GetBoatClassProperty("GetProtectedNoSetProperty").IsInternal);
            Assert.IsFalse(GetBoatClassProperty("GetProtectedNoGetProperty").IsInternal);

            // Standalone Internal
            Assert.IsTrue(GetBoatClassProperty("InternalProperty").IsInternal);
            Assert.IsFalse(GetBoatClassProperty("InternalGetProperty").IsInternal);
            Assert.IsFalse(GetBoatClassProperty("InternalSetProperty").IsInternal);
            Assert.IsTrue(GetBoatClassProperty("InternalNoSetProperty").IsInternal);
            Assert.IsTrue(GetBoatClassProperty("InternalNoGetProperty").IsInternal);

            // Internal & Protected
            Assert.IsTrue(GetBoatClassProperty("InternalProtectedProperty").IsInternal);
            Assert.IsFalse(GetBoatClassProperty("InternalProtectedGetProperty").IsInternal);
            Assert.IsFalse(GetBoatClassProperty("InternalProtectedSetProperty").IsInternal);
            Assert.IsTrue(GetBoatClassProperty("InternalProtectedNoSetProperty").IsInternal);
            Assert.IsTrue(GetBoatClassProperty("InternalProtectedNoGetProperty").IsInternal);

            // Static & Internal & Protected
            Assert.IsTrue(GetBoatClassProperty("StaticInternalProtectedProperty").IsInternal);
            Assert.IsTrue(GetBoatClassProperty("StaticInternalProtectedNoGetProperty").IsInternal);
            Assert.IsTrue(GetBoatClassProperty("StaticInternalProtectedNoSetProperty").IsInternal);
        }

        [Test(Description = "Ensures the <IsPublic> member of the <PropertyModel> type is set correctly.")]
        public void IsPublic()
        {
            Assert.IsTrue(GetBoatClassProperty(nameof(Boat.NoGetterProperty)).IsPublic);
        }

        PropertyModel GetBoatClassProperty(string prop)
            => GetPropertyModel(nameof(Data.Classes), nameof(Boat), prop);

        public PropertyModel GetPropertyModel(string _namespace, string className, string propName)
            => (Docs.Builder.ProjectManager.RootProject.Codebase.Root
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces[_namespace]
                .Types[className]
                .Member as IMemberContainable)
            .Properties
            .Single(e => e.Name == propName);
    }
}
