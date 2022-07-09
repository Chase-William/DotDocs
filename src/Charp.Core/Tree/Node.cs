using System;
using System.Linq;
using System.Collections.Generic;
using Charp.Core.Models;
using System.Reflection;

namespace Charp.Core.Tree
{
    /// <summary>
    /// The most basic building block of all node types.
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// The node this node branches off of.
        /// </summary>
        public Node Parent { get; private set; }

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        /// <returns>Name of node.</returns>
        public abstract string GetName();

        /// <summary>
        /// Initializes a new instance of <see cref="Node"/> with a parent node set.
        /// </summary>
        /// <param name="parent">Node this node branches off of.</param>
        protected Node(Node parent)
            => Parent = parent;

        /// <summary>
        /// Begins the process of writing this node to file.
        /// </summary>
        /// <param name="outputPath">Location to write the node to.</param>
        /// <param name="namespaces">Namespace trace of this node.</param>
        /// <param name="nestables">Nestable trace of this node.</param>
        public abstract void Save(string outputPath, Stack<string> namespaces, Stack<string> nestables);       

        /// <summary>
        /// Joins namespaces into a path.
        /// </summary>
        /// <param name="namespaces">To be joined.</param>
        /// <returns>Joined namespaces.</returns>
        protected static string JoinNamespaces(Stack<string> namespaces)
            => string.Join('\\', namespaces.ToArray().Reverse());        

        /// <summary>
        /// Joins nestables into a valid filename.
        /// </summary>
        /// <param name="nestables">To be joined.</param>
        /// <returns>Joined nestables.</returns>
        protected static string JoinNestables(Stack<string> nestables)
            => string.Join('+', nestables.ToArray().Reverse());
    }
}
