using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Docsharp.Core.Models;
using Docsharp.Core.Models.Docs;
using System.IO;

namespace Docsharp.Core.Tree
{   
    public class NamespaceNode : Node, ITypeNodeNestable
    {
        private readonly string _namespace;

        public Dictionary<string, NamespaceNode> Namespaces { get; set; } = new();
        public Dictionary<string, TypeNode> Types { get; set; } = new();

        public NamespaceNode(NamespaceNode parent, string _namespace) : base(parent)
        {
            this._namespace = _namespace;
        }

        public override string GetName()
            => _namespace;

        public void AddType(ArraySegment<string> segments, TypeMember<TypeInfo, Documentation> member)
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

        public override void Save(Stack<string> namespaces, Stack<string> nestables)
        {  
            /* 
             * Push this namespace to the stack because everything listed
             * below lives under it
             */
            namespaces.Push(GetName());
            // Create directory if needed
            Directory.CreateDirectory(JoinNamespaces(namespaces));
                    
            // Save namespaces recursively
            foreach (NamespaceNode node in Namespaces.Values)
                node.Save(namespaces, nestables);
            // Save types recursively
            foreach (Node node in Types.Values)
                node.Save(namespaces, nestables);

            // Pop this namespace when leaving as we are traversing back up the tree                   
            namespaces.Pop();
        }

        public TypeMember<TypeInfo, Documentation> FindType(ArraySegment<string> segments)
        {
            if (segments.Count == 1)
            {
                segments = segments[0].Split('+');
                // Nested types are present
                if (segments.Count > 1)
                    return ((TypeNodeNestable)Types[segments[0]]).FindType(segments[1..]);
                return Types[segments[0]].Member;
            }

            return Namespaces[segments[0]].FindType(segments[1..]);
        }
    }
}
