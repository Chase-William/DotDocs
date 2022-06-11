using Charp.Core.Models;
using Charp.Core.Models.Members;
using Charp.Test.Data.Classes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charp.Test.Members
{
    internal class PropertyTest : BaseTest
    {
        [Test(Description = "Ensures the <CanWrite> member of the <PropertyModel> type is set correctly.")]
        public void CanWriteSetCorrectly()
        {
            Assert.IsTrue(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Length)).CanSet);
            Assert.IsTrue(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Weight)).CanSet);
            Assert.IsFalse(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.NoSetterProperty)).CanSet);
            // No private events are currently exposed
        }

        [Test(Description = "Ensures the <CanRead> member of the <PropertyModel> type is set correctly.")]
        public void CanReadSetCorrectly()
        {
            Assert.IsTrue(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Length)).CanGet);
            Assert.IsTrue(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Weight)).CanGet);
            Assert.IsFalse(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.NoGetterProperty)).CanGet);
            // No private events are currently exposed
        }

        [Test(Description = "Ensures the <IsSetPrivate> member of the <PropertyModel> type is set correctly.")]
        public void IsSetPrivateSetCorrectly()
        {
            Assert.IsFalse(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Length)).IsSetPrivate);
            Assert.IsFalse(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Weight)).IsSetPrivate);
            Assert.IsTrue(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.PrivateSetterProperty)).IsSetPrivate);
            // No private events are currently exposed
        }

        [Test(Description = "Ensures the <IsGetPrivate> member of the <PropertyModel> type is set correctly.")]
        public void IsGetPrivateSetCorrectly()
        {
            Assert.IsFalse(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Length)).IsGetPrivate);
            Assert.IsFalse(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Weight)).IsGetPrivate);
            Assert.IsTrue(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.PrivateGetterProperty)).IsGetPrivate);
            // No private events are currently exposed
        }

        [Test(Description = "Ensures the <IsVirtual> member of the <PropertyModel> type is set correctly.")]
        public void IsVirtualSetCorrectly()
        {
            Assert.IsFalse(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Length)).IsVirtual);
            Assert.IsFalse(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Weight)).IsVirtual);
        }

        [Test(Description = "Ensures the <IsAbstract> member of the <PropertyModel> type is set correctly.")]
        public void IsAbstractSetCorrectly()
        {
            Assert.IsFalse(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Length)).IsVirtual);
            Assert.IsFalse(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Weight)).IsVirtual);
        }

        [Test(Description = "Ensures the <IsAbstract> member of the <PropertyModel> type is set correctly.")]
        public void IsStaticSetCorrectly()
        {
            Assert.IsFalse(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Length)).IsVirtual);
            Assert.IsFalse(GetPropertyModel(nameof(Data.Classes), nameof(Boat), nameof(Boat.Weight)).IsVirtual);
        }

        public PropertyModel GetPropertyModel(string _namespace, string className, string propName)
            => (Docs.Models.Root
                .Namespaces["Test"]
                .Namespaces["Data"]
                .Namespaces[_namespace]
                .Types[className]
                .Member as INestable)
            .Properties
            .Single(e => e.Name == propName);
    }
}
