using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Docsharp.Core.Tree;

namespace Docsharp.Test.Interfaces.Nodes
{
    /// <summary>
    /// Ensures the aspects of <see cref="ITypeContainable"/> operate as expected.
    /// </summary>
    internal interface ITypeContainableTest : ITypeNodeTest
    {
        /// <summary>
        /// Ensures contained types within a <see cref="ITypeContainable"/> exist.
        /// </summary>
        void ContainedTypesExist();

        TypeContainable GetNodeType(string name);
    }
}
