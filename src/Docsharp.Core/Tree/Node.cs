using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docsharp.Core.Tree
{
    public abstract class Node
    {
        public Node Parent { get; private set; }

        public abstract string GetName();

        protected Node(Node parent)
            => Parent = parent;
    }
}
