using Docshark.Core.Global.Types;
using Docshark.Test.Global.Types.Data;
using Docshark.Test.Global.Types.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Docshark.Test.Global.Types
{
    internal class CompoundSystemTypeTest : ITypeTest, ICompoundTest
    {
        Type action;
        Type func;
        Type dict;
        TypeMapper map;

        [OneTimeSetUp]
        public void Setup()
        {
            action = typeof(Action<string, int>);
            func = typeof(KeyValuePair<string, char>);
            dict = typeof(Dictionary<long, ChildClass>);
            map = new TypeMapper();
            map.AddType(action);
            map.AddType(func);
            map.AddType(dict);
        }

        #region ITypeTest

        [Test(Description = "Ensures compound types themselves are added.")]
        public void TypeAdded()
        {
            Assert.IsTrue(map.MappedDefinitions.ContainsKey(action.ToString()));
            Assert.IsTrue(map.MappedDefinitions.ContainsKey(func.ToString()));
            Assert.IsTrue(map.MappedDefinitions.ContainsKey(dict.ToString()));
        }

        [Test(Description = "Ensures type dependencies of the compound type's parent are added. This excludes arguments.")]
        public void InDirectParentTypesAdded()
        {
            Assert.IsTrue(map.MappedDefinitions.ContainsKey(typeof(object).ToString()));
            Assert.IsTrue(map.MappedDefinitions.ContainsKey(typeof(Delegate).ToString()));
            Assert.IsTrue(map.MappedDefinitions.ContainsKey(typeof(MulticastDelegate).ToString()));
        }

        [Test(Description = "Ensures direct parents of the compound types are added.")]
        public void DirectTypeParentAdded()
        {
            Assert.NotNull(map.MappedDefinitions[action.ToString()].BaseType);
            Assert.NotNull(map.MappedDefinitions[func.ToString()].BaseType);
            Assert.NotNull(map.MappedDefinitions[dict.ToString()].BaseType);
        }

        [Test(Description = "Ensures indirect parents of the compound types are added.")]
        public void InDirectTypeParentsAdded()
        {
            Assert.IsNull(map.MappedDefinitions[typeof(object).ToString()].BaseType);
            Assert.NotNull(map.MappedDefinitions[typeof(Delegate).ToString()].BaseType);
            Assert.NotNull(map.MappedDefinitions[typeof(MulticastDelegate).ToString()].BaseType);
        }

        #endregion

        #region ICompoundTest

        [Test(Description = "Ensures direct type arguments are added.")]
        public void DirectTypeArgumentsAddedToDictionary()
        {
            Assert.NotNull(map.MappedDefinitions[typeof(string).ToString()]);
            Assert.NotNull(map.MappedDefinitions[typeof(int).ToString()]);
            Assert.NotNull(map.MappedDefinitions[typeof(char).ToString()]);
            Assert.NotNull(map.MappedDefinitions[typeof(long).ToString()]);
            Assert.NotNull(map.MappedDefinitions[typeof(ChildClass).ToString()]);
        }

        [Test(Description = "Ensures indirect type arguments are added.")]
        public void InDirectTypeArgumentsAddedToDictionary()
        {
            Assert.NotNull(map.MappedDefinitions[typeof(ValueType).ToString()]);
            Assert.NotNull(map.MappedDefinitions[typeof(ParentClass).ToString()]);
        }

        [Test(Description = "Ensures type arguments are added to the type argument list of the respective compound type.")]
        public void TypeArgumentsAddedToArgumentList()
        {
            Assert.NotNull(map.MappedDefinitions[action.ToString()].TypeArguments.Contains(typeof(string).ToString()));
            Assert.NotNull(map.MappedDefinitions[action.ToString()].TypeArguments.Contains(typeof(int).ToString()));

            Assert.NotNull(map.MappedDefinitions[func.ToString()].TypeArguments.Contains(typeof(string).ToString()));
            Assert.NotNull(map.MappedDefinitions[func.ToString()].TypeArguments.Contains(typeof(char).ToString()));

            Assert.NotNull(map.MappedDefinitions[dict.ToString()].TypeArguments.Contains(typeof(long).ToString()));
            Assert.NotNull(map.MappedDefinitions[dict.ToString()].TypeArguments.Contains(typeof(string).ToString()));
        }

        [Test(Description = "Ensures type arguments are not duplicated.")]
        public void TypeArgumentNotDuplicated()
        {
            Assert.AreEqual(2, map.MappedDefinitions[action.ToString()].TypeArguments.Count);
            Assert.AreEqual(2, map.MappedDefinitions[func.ToString()].TypeArguments.Count);
            Assert.AreEqual(2, map.MappedDefinitions[dict.ToString()].TypeArguments.Count);
        }

        #endregion
    }
}
