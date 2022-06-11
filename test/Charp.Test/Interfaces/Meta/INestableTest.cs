using System;
using System.Linq;
using System.Reflection;

using Charp.Core;
using Charp.Core.Models;
using Charp.Core.Models.Types;
using Charp.Core.Tree;

namespace Charp.Test.Interfaces.Meta
{
    /// <summary>
    /// Ensures the aspects of <see cref="INestableTest"/> operate as expected.
    /// </summary>
    internal interface INestableTest
    {
        /// <summary>
        /// Ensures the existance/absense of properties are handled correctly.
        /// </summary>
        void PropertiesExistTest();
        /// <summary>
        /// Ensures the existance/absense of fields are handled correctly.
        /// (THIS TEST EXPECTS BACKING FIELDS OF PROPERTIES TO BE OMITTED)
        /// </summary>
        void FieldsExistTest();
        /// <summary>
        /// Ensures the existance/absense of methods are handled correctly.
        /// (THIS TEST EXPECTS PRIVATE & MEMBERS System.Object TO BE OMITTED)
        /// </summary>
        void MethodsExistTest();

        /// <summary>
        /// Implement to get the class type provided via <paramref name="className"/>.
        /// </summary>
        /// <param name="className">Class type as a string to get acquired.</param>
        /// <returns>A class type that implements <see cref="INestable"/>.</returns>
        T GetType<T>(string className) where T : INestable;

        /// <summary>
        /// Returns count of fields declared in the class.
        /// </summary>
        /// <param name="type">Type to get fields from.</param>
        /// <returns>Declared fields from instance within <paramref name="type"/>.</returns>
        static int GetFieldCount(Type type)
            => type.GetTypeInfo().GetFields().Length;

        /// <summary>
        /// Returns count of public methods declared in the class. This ignores
        /// generated property getter/setters.
        /// </summary>
        /// <param name="type">Type to get methods from.</param>
        /// <returns>Declared methods from instance within <paramref name="type"/>.</returns>
        static int GetMethodCount(Type type)
            => type.GetTypeInfo().GetMethods().Where(method => !method.IsSpecialName).Count();

        /// <summary>
        /// Returns count of properties declared in the class.
        /// </summary>
        /// <param name="type">Type to get properties from.</param>
        /// <returns>Declared properties from instance within <paramref name="type"/>.</returns>
        static int GetPropertyCount(Type type)
            => type.GetTypeInfo().GetProperties().Length;
    }
}
