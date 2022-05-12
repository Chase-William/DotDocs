using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Docsharp.Core.Models;
using Docsharp.Core.Models.Docs;

namespace Docsharp.Core.Tree
{
    /// <summary>
    /// A tree of namespaces and types created lazily that represents the structure of one's source code.
    /// </summary>
    public class MetadataTree
    {
        public NamespaceNode Root { get; private set; }

        public MetadataTree(string rootNamespace)
            => Root = new NamespaceNode(null, rootNamespace);

        /// <summary>
        /// Adds a new type under the provided namespace.
        /// </summary>
        /// <param name="fullNamespaceToType"></param>
        public void AddType(string fullNamespaceToType, TypeMember<TypeInfo, Documentation> member)
        {
            // We know the last item in this string[] is our type
            // Everything before it, a namespace            
            var segments = fullNamespaceToType.Split(".");

            ArraySegment<string> a = segments;

            Root.AddType(a, member);
        }
    }
}
