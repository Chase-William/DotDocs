using Docshark.Core.Global.Types;
using Docshark.Test.Global.Types.Interfaces;
using NUnit.Framework;
using System;

namespace Docshark.Test.Global.Types
{
    internal class SimpleTypeTest : ITypeTest
    {
        TypeMapper map;

        [OneTimeSetUp]
        public void Setup()
        {
            map = new TypeMapper();
            map.AddType(typeof(string));
            map.AddType(typeof(int));
        }

        [Test(Description = "Ensures types directly added to the TypeMap are indeed added.")]
        public void TypeAdded()
        {
            Assert.True(map.Types.ContainsKey(typeof(string).ToString()));
            Assert.True(map.Types.ContainsKey(typeof(int).ToString()));
        }

        [Test(Description = "Ensures types added indirectly or as a by product of another type being added are added.")]
        public void InDirectParentTypesAdded()
        {
            Assert.True(map.Types.ContainsKey(typeof(object).ToString()));
            Assert.True(map.Types.ContainsKey(typeof(ValueType).ToString()));
        }

        [Test(Description = "Ensures direct parent types are added.")]
        public void DirectTypeParentAdded()
        {
            Assert.NotNull(map.Types[typeof(string).ToString()].Parent);
            Assert.NotNull(map.Types[typeof(int).ToString()].Parent);
        }

        [Test(Description = "Ensures indirect parent types are added.")]
        public void InDirectTypeParentsAdded()
        {
            Assert.NotNull(map.Types[typeof(ValueType).ToString()].Parent);
            Assert.IsNull(map.Types[typeof(object).ToString()].Parent);
        }
    }
}
