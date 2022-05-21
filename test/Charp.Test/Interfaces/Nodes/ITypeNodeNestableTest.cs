using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Charp.Core.Tree;

namespace Charp.Test.Interfaces.Nodes
{
    /// <summary>
    /// Ensures the aspects of <see cref="ITypeNodeNestable"/> operate as expected.
    /// </summary>
    internal interface ITypeNodeNestableTest : ITypeNodeTest
    {
        /// <summary>
        /// Ensures contained types within a <see cref="ITypeNodeNestable"/> exist.
        /// </summary>
        void ContainedTypesExist();

        TypeNodeNestable GetNodeType(string name);
    }
}
