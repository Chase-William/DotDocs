using System;
using System.Linq;
using System.Collections.Generic;

namespace Docsharp.Core.Tree
{
    public abstract class Node
    {
        public Node Parent { get; private set; }

        public abstract string GetName();

        protected Node(Node parent)
            => Parent = parent;

        public abstract void Save(Stack<string> namespaces, Stack<string> nestables);

        //public static String Join(String? separator, String?[] value, int startIndex, int count);
        protected static string JoinNamespaces(Stack<string> namespaces)
            => string.Join('\\', namespaces.ToArray().Reverse());        

        protected static string JoinNestables(Stack<string> nestables)
            => string.Join('+', nestables.ToArray().Reverse());
    }
}
