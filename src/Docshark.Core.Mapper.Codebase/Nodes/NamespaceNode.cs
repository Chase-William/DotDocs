using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using LoxSmoke.DocXml;
using Docshark.Core.Models.Codebase;
using Docshark.Core.Models.Codebase.Types;

namespace Docshark.Core.Mapper.Codebase.Nodes
{
    /// <summary>
    /// Represents a namespace as a node.
    /// </summary>
    public class NamespaceNode : Node, ITypeNodeNestable
    {
        /// <summary>
        /// Namespace string for this <see cref="NamespaceNode"/>.
        /// </summary>
        private readonly string _namespace;

        /// <summary>
        /// Contains all the namespaces containe within this <see cref="NamespaceNode"/>.
        /// </summary>
        public Dictionary<string, NamespaceNode> Namespaces { get; set; } = new();

        /// <summary>
        /// Contains all the types within this <see cref="NamespaceNode"/>.
        /// </summary>
        public Dictionary<string, TypeNode> Types { get; set; } = new();

        /// <summary>
        /// Initializes a new instance of <see cref="NamespaceNode"/>.
        /// </summary>
        /// <param name="parent">Node this node branches off of.</param>
        /// <param name="_namespace">Name as a string for this node.</param>
        public NamespaceNode(NamespaceNode parent, string _namespace) : base(parent)
            => this._namespace = _namespace;

        /// <summary>
        /// Gets the namespace name.
        /// </summary>
        /// <returns>Namespace's name.</returns>
        public override string GetName()
            => _namespace;

        /// <summary>
        /// Adds a given type to either this node or one of it's descendents in a recursive fashion.
        /// </summary>
        /// <param name="segments">Namespace & type trace leading to where the type should be added.</param>
        /// <param name="member">Information about the type.</param>
        public void AddType(ArraySegment<string> segments, TypeMember<TypeInfo, TypeComments> member)
        {
            string name = segments.First();

            // End of namespace chain reached, now currently at a Type
            if (segments.Count == 1)
            {
                /**
                 * Handle case where only one type is present (no types nested within types)
                 */

                segments = name.Split("+");
                name = segments.First();
                // Our last iteration will be some kind of Type               
                if (segments.Count == 1)
                {
                    // Case for a type that contain internally defined types
                    if (member.CanHaveInternalTypes)
                    {
                        Types.Add(name, new TypeNodeNestable(this, member));
                        return;
                    }
                    // Add a type that cannot contain other internally defined types                  
                    Types.Add(name, new TypeNode(this, member));
                    return;

                    // Should return to caller in this case without further execution
                }

                /**
                 * Handle case where there are types nested within types
                 */

                // Key for namespace already exist, therefore get a ref and add type
                if (Types.ContainsKey(name))
                {
                    ((ITypeNodeNestable)Types[name]).AddType(segments[1..segments.Count], member);
                    return;
                }
                var typeNode = new TypeNodeNestable(this, member);
                Types.Add(name, typeNode);
                // Now iterate through nested type chain
                typeNode.AddType(segments[1..segments.Count], member);
                return;
            }

            if (!Namespaces.ContainsKey(name))
            {
                Namespaces.Add(name, new NamespaceNode(this, name));
            }
            segments = segments[1..segments.Count];
            Namespaces[name].AddType(segments, member);
        }

        /// <summary>
        /// Begins the process of writing this <see cref="NamespaceNode"/> and all its <see cref="Namespaces"/> & <see cref="Types"/> to file.
        /// </summary>
        /// <param name="outputPath">Location to write to.</param>
        /// <param name="namespaces">Namespace trace.</param>
        /// <param name="nestables">Nestable trace.</param>
        public override void Save(string outputPath, Stack<string> namespaces, Stack<string> nestables)
        {
            /* 
             * Push this namespace to the stack because everything listed
             * below lives under it
             */
            namespaces.Push(GetName());

            // Create directory if needed
            Directory.CreateDirectory(Path.Combine(outputPath, JoinNamespaces(namespaces)));
            var easy = Types.Values;
            Node mnode;
            // Save namespaces recursively
            foreach (NamespaceNode node in Namespaces.Values)
                node.Save(outputPath, namespaces, nestables);
            try
            {
                // Save types recursively
                foreach (Node node in Types.Values)
                {
                    mnode = node;
                    node.Save(outputPath, namespaces, nestables);
                }
            }
            catch (Exception ex)
            {
                // reading in types that are not in this assembly will mess things up homie as they've already been loaded
                // maybe.. make and save this before doing it sub dependencies? idk
                Console.WriteLine();
            }

            // Pop this namespace when leaving as we are traversing back up the tree                   
            namespaces.Pop();
        }

        /// <summary>
        /// Searches for a type that branches of this <see cref="NamespaceNode"/>.
        /// </summary>
        /// <param name="segments">Location of the type.</param>
        /// <returns>The found type.</returns>
        public TypeMember<TypeInfo, TypeComments> FindType(ArraySegment<string> segments)
        {
            string first = segments[0];

            // If we reached the last segment in our collection, it must be the type            
            if (segments.Count == 1)
                return Types[segments[0]].Member;

            // Check if next segment is a namespace or type
            if (!Namespaces.ContainsKey(first))
                // Start the nested Types recursion search
                return ((TypeNodeNestable)Types[first]).FindType(segments[1..]);
            // Check the next nested namespace
            return Namespaces[first].FindType(segments[1..]);
        }

        /// <summary>
        /// Searches for a field that branches off this <see cref="NamespaceNode"/>.
        /// </summary>
        /// <param name="segments"></param>
        /// <returns></returns>
        public Model<FieldInfo, CommonComments> FindField(ArraySegment<string> segments)
        {
            string first = segments[0];

            // Check if we have reached the last type before the field
            // Catching this case early also simplifies handling enumerations (uses TypeNode)
            if (segments.Count == 2)
                return ((IFieldable)Types[first].Member).Fields.FirstOrDefault(f => f.Name.Equals(segments[1]));

            // Check if next segment is a namespace or type
            if (!Namespaces.ContainsKey(first))
                // Start the nested Types recursion search
                return ((TypeNodeNestable)Types[first]).FindField(segments[1..]);
            // Check the next nested namespace
            return Namespaces[first].FindField(segments[1..]);
        }

        /// <summary>
        /// Searches for a property that branches of this <see cref="NamespaceNode"/>.
        /// </summary>
        /// <param name="segments"></param>
        /// <returns></returns>
        public Model<PropertyInfo, CommonComments> FindProperty(ArraySegment<string> segments)
        {
            string first = segments[0];

            // Check if next segment is a namespace or type
            if (!Namespaces.ContainsKey(first))
                // Start the nested Types recursion search
                return ((TypeNodeNestable)Types[first]).FindProperty(segments[1..]);
            // Check the next nested namespace
            return Namespaces[first].FindProperty(segments[1..]);
        }

        /// <summary>
        /// Searches for an event that branches off this <see cref="NamespaceNode"/>.
        /// </summary>
        /// <param name="segments"></param>
        /// <returns></returns>
        public Model<EventInfo, CommonComments> FindEvent(ArraySegment<string> segments)
        {
            string first = segments[0];

            // Check if next segment is a namespace or type
            if (!Namespaces.Keys.Contains(first))
                // Start the nested Types recursion search
                return ((TypeNodeNestable)Types[first]).FindEvent(segments[1..]);
            // Check the next nested namespace
            return Namespaces[first].FindEvent(segments[1..]);
        }
    }
}
