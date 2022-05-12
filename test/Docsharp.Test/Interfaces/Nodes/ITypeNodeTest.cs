using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Docsharp.Core.Tree;

namespace Docsharp.Test.Interfaces.Nodes
{
    /// <summary>
    /// Ensures the aspects of <see cref="TypeNode"/> operate as expected.
    /// </summary>
    internal interface ITypeNodeTest : INodeTest
    {
        /// <summary>
        /// Ensures the <see cref="TypeNode.Member"/> is never null.
        /// </summary>
        public void MemberIsNotNull();
    }
}
