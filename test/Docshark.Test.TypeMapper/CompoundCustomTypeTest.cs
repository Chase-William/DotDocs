using Docshark.Test.TypeMapper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docshark.Test.TypeMapper.Data;
using NUnit.Framework;
using Docshark.Core.TypeMapper;

namespace Docshark.Test.TypeMapper
{
    internal class CompoundCustomTypeTest : ITypeTest, ICompoundTest
    {
        Type argumentedClass;
        Type childOfArgumentedClass;
        TypeMap map;

        #region ITypeTest

        [OneTimeSetUp]
        public void Setup()
        {
            argumentedClass = typeof(ArgumentedClass<LeftArgument, RightArgument>);
            childOfArgumentedClass = typeof(ChildOfArgumentedClass);
            map = new TypeMap();
            map.AddType(argumentedClass);
            map.AddType(childOfArgumentedClass);
        }

        [Test(Description = "Ensures direct parents of the compound types are added.")]
        public void DirectTypeParentAdded()
        {
            Assert.NotNull(map.Types[argumentedClass.ToString()].Parent);
        }

        [Test(Description = "Ensures type dependencies of the compound type's parent are added. This excludes arguments.")]
        public void InDirectParentTypesAdded()
        {
            Assert.IsTrue(map.Types.ContainsKey(typeof(object).ToString()));
        }

        [Test(Description = "Ensures indirect parents of the compound types are added.")]
        public void InDirectTypeParentsAdded()
        {
            Assert.IsNull(map.Types[typeof(object).ToString()].Parent);
            Assert.NotNull(map.Types[typeof(ParentClass).ToString()].Parent);
        }

        [Test(Description = "Ensures compound types themselves are added.")]
        public void TypeAdded()
        {
            Assert.IsTrue(map.Types.ContainsKey(argumentedClass.ToString()));
        }

        #endregion

        #region ICompoundTest

        [Test(Description = "Ensures direct type arguments are added.")]
        public void DirectTypeArgumentsAddedToDictionary()
        {
            Assert.NotNull(map.Types[typeof(LeftArgument).ToString()]);
            Assert.NotNull(map.Types[typeof(RightArgument).ToString()]);
        }

        [Test(Description = "Ensures type arguments are added to the type argument list of the respective compound type.")]
        public void TypeArgumentsAddedToArgumentList()
        {
            Assert.NotNull(map.Types[argumentedClass.ToString()].TypeArguments.Contains(typeof(LeftArgument).ToString()));
            Assert.NotNull(map.Types[argumentedClass.ToString()].TypeArguments.Contains(typeof(RightArgument).ToString()));            
        }

        [Test(Description = "Ensures indirect type arguments are added.")]
        public void InDirectTypeArgumentsAddedToDictionary()
        {
            Assert.NotNull(map.Types[typeof(LeftArgumentParent).ToString()]);
            Assert.NotNull(map.Types[typeof(object).ToString()]);
        }

        [Test(Description = "Ensures type arguments are not duplicated.")]
        public void TypeArgumentNotDuplicated()
        {
            Assert.AreEqual(2, map.Types[argumentedClass.ToString()].TypeArguments.Count);
            Assert.AreEqual(0, map.Types[childOfArgumentedClass.ToString()].TypeArguments.Count);
        }

        #endregion
    }
}
