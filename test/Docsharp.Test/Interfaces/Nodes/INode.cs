using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Docsharp.Core.Tree;

namespace Docsharp.Test.Interfaces.Nodes
{
    /// <summary>
    /// Ensures the aspects of <see cref="Node"/> operate correctly.
    /// </summary>
    internal interface INodeTest
    {
        /// <summary>
        /// Ensures the <see cref="Node.GetName"/> method returns a string
        /// matching the type name it contains.
        /// </summary>
        public void GetNameMatchesTypeName();

        /// <summary>
        /// Ensures the <see cref="Node.Parent"/> is not null.
        /// </summary>
        public void ParentNodeNotNull();
    }
}
