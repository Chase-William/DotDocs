using Docshark.Core.TypeMapper;
using Docshark.Test.TypeMapper.Data;
using Docshark.Test.TypeMapper.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Test.TypeMapper
{
    internal class CompoundSystemTypeTest : ITypeTest, ICompoundTest
    {
        Type action;
        Type func;
        Type dict;
        TypeMap map;

        [OneTimeSetUp]
        public void Setup()
        {
            action = typeof(Action<string, int>);
            func = typeof(KeyValuePair<string, char>);
            dict = typeof(Dictionary<long, ChildClass>);
            map = new TypeMap();
            map.AddType(action);
            map.AddType(func);
            map.AddType(dict);
        }

        #region ITypeTest

        [Test(Description = "Ensures compound types themselves are added.")]
        public void TypeAdded()
        {
            Assert.IsTrue(map.Types.ContainsKey(action.ToString()));
            Assert.IsTrue(map.Types.ContainsKey(func.ToString()));
            Assert.IsTrue(map.Types.ContainsKey(dict.ToString()));
        }

        [Test(Description = "Ensures type dependencies of the compound type's parent are added. This excludes arguments.")]
        public void InDirectParentTypesAdded()
        {            
            Assert.IsTrue(map.Types.ContainsKey(typeof(object).ToString()));
            Assert.IsTrue(map.Types.ContainsKey(typeof(Delegate).ToString()));
            Assert.IsTrue(map.Types.ContainsKey(typeof(MulticastDelegate).ToString()));
        }

        [Test(Description = "Ensures direct parents of the compound types are added.")]
        public void DirectTypeParentAdded()
        {
            Assert.NotNull(map.Types[action.ToString()].Parent);
            Assert.NotNull(map.Types[func.ToString()].Parent);
            Assert.NotNull(map.Types[dict.ToString()].Parent);
        }

        [Test(Description = "Ensures indirect parents of the compound types are added.")]
        public void InDirectTypeParentsAdded()
        {            
            Assert.IsNull(map.Types[typeof(object).ToString()].Parent);
            Assert.NotNull(map.Types[typeof(Delegate).ToString()].Parent);
            Assert.NotNull(map.Types[typeof(MulticastDelegate).ToString()].Parent);
        }

        #endregion

        #region ICompoundTest

        [Test(Description = "Ensures direct type arguments are added.")]
        public void DirectTypeArgumentsAddedToDictionary()
        {
            Assert.NotNull(map.Types[typeof(string).ToString()]);
            Assert.NotNull(map.Types[typeof(int).ToString()]);
            Assert.NotNull(map.Types[typeof(char).ToString()]);
            Assert.NotNull(map.Types[typeof(long).ToString()]);
            Assert.NotNull(map.Types[typeof(ChildClass).ToString()]);
        }

        [Test(Description = "Ensures indirect type arguments are added.")]
        public void InDirectTypeArgumentsAddedToDictionary()
        {
            Assert.NotNull(map.Types[typeof(ValueType).ToString()]);
            Assert.NotNull(map.Types[typeof(ParentClass).ToString()]);
        }

        [Test(Description = "Ensures type arguments are added to the type argument list of the respective compound type.")]
        public void TypeArgumentsAddedToArgumentList()
        {
            Assert.NotNull(map.Types[action.ToString()].TypeArguments.Contains(typeof(string).ToString()));
            Assert.NotNull(map.Types[action.ToString()].TypeArguments.Contains(typeof(int).ToString()));

            Assert.NotNull(map.Types[func.ToString()].TypeArguments.Contains(typeof(string).ToString()));
            Assert.NotNull(map.Types[func.ToString()].TypeArguments.Contains(typeof(char).ToString()));

            Assert.NotNull(map.Types[dict.ToString()].TypeArguments.Contains(typeof(long).ToString()));
            Assert.NotNull(map.Types[dict.ToString()].TypeArguments.Contains(typeof(string).ToString()));
        }

        [Test(Description = "Ensures type arguments are not duplicated.")]
        public void TypeArgumentNotDuplicated()
        {
            Assert.AreEqual(2, map.Types[action.ToString()].TypeArguments.Count);
            Assert.AreEqual(2, map.Types[func.ToString()].TypeArguments.Count);
            Assert.AreEqual(2, map.Types[dict.ToString()].TypeArguments.Count);
        }

        #endregion
    }
}
