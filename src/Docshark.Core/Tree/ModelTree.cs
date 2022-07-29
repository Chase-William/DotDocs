using System;
using System.Collections.Generic;
using System.Reflection;
using Docshark.Core.Models.Lang;
using Docshark.Core.Models.Lang.Types;
using LoxSmoke.DocXml;

namespace Docshark.Core.Tree
{
    /// <summary>
    /// A tree of namespaces and types created lazily that represents the structure of one's source code.
    /// </summary>
    public class ModelTree
    {
        /// <summary>
        /// The entry point node.
        /// </summary>
        public NamespaceNode Root { get; private set; }

        /// <summary>
        /// Intializes a new empty instance of <see cref="ModelTree"/>.
        /// </summary>
        public ModelTree() { }

        /// <summary>
        /// Adds a new type under the provided namespace.
        /// </summary>
        /// <param name="fullNamespaceToType"></param>
        public void AddType(string fullNamespaceToType, TypeMember<TypeInfo, TypeComments> member)
        {
            // We know the last item in this string[] is our type
            // Everything before it, a namespace            
            var segments = fullNamespaceToType.Split(".");

            if (Root == null)
                Root = new NamespaceNode(null, segments[0]);

            Root.AddType(segments[1..], member);
        }

        /// <summary>
        /// Searches for a type in the tree.
        /// </summary>
        /// <param name="fullName">The full namespace and type name leading to the target.</param>
        /// <returns>The type.</returns>
        public TypeMember<TypeInfo, TypeComments> FindType(string fullName)
        {
            ArraySegment<string> segments = fullName.Split('.');
            return Root.Namespaces[segments[1]].FindType(segments[2..]);
        }

        /// <summary>
        /// Searches for a field in the tree.
        /// </summary>
        /// <param name="fullName">The full namespace and type name leading to the target.</param>
        /// <returns>The field.</returns>
        public Model<FieldInfo, CommonComments> FindField(string fullName)
        {
            ArraySegment<string> segments = fullName.Split('.');
            return Root.Namespaces[segments[1]].FindField(segments[2..]);
        }

        /// <summary>
        /// Searches for a property in the tree.
        /// </summary>
        /// <param name="fullName">The full namespace and type name leading to the target.</param>
        /// <returns>The property.</returns>
        public Model<PropertyInfo, CommonComments> FindProperty(string fullName)
        {
            ArraySegment<string> segments = fullName.Split('.');
            return Root.Namespaces[segments[1]].FindProperty(segments[2..]);
        }

        /// <summary>
        /// Searches for an event in the tree.
        /// </summary>
        /// <param name="fullName">The full namespace and type name leading to the target.</param>
        /// <returns>The event.</returns>
        public Model<EventInfo, CommonComments> FindEvent(string fullName)
        {
            ArraySegment<string> segments = fullName.Split('.');
            return Root.Namespaces[segments[1]].FindEvent(segments[2..]);
        }

        /// <summary>
        /// Begins the process of iterating through the tree and writing all nodes to file.
        /// </summary>
        /// <param name="outputPath">Location to write from.</param>
        public void SaveModels(string outputPath)
        {
            Utility.CleanDirectory(outputPath);
            Root.Save(outputPath, new Stack<string>(), new Stack<string>());
        }            
    }
}
