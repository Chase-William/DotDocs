using Docsharp.Core.Models;
using Docsharp.Core.Models.Docs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Docsharp.Core.Tree
{
    public class TypeNodeNestable : TypeNode, ITypeNodeNestable
    {
        public Dictionary<string, TypeNode> Types { get; set; } = new();

        public TypeNodeNestable(Node parent, TypeMember<TypeInfo, Documentation> member) : base(parent, member)
        { }

        public void AddType(ArraySegment<string> types, TypeMember<TypeInfo, Documentation> member)
        {
            string typeName = types.First();
            // End of nested type chain reached
            if (types.Count == 1)
            {
                if (member.CanHaveInternalTypes)
                {
                    Types.Add(typeName, new TypeNodeNestable(this, member));
                    return;
                }
                Types.Add(typeName, new TypeNode(this, member));
                return;
            }

            TypeNodeNestable type;
            if (!Types.ContainsKey(typeName))
            {
                type = new TypeNodeNestable(this, null);
                Types.Add(typeName, type);
            }
            else
                type = (TypeNodeNestable)Types[typeName];
            types = types[1..types.Count];
            type.AddType(types, member);
        }        

        /// <summary>
        /// Iterate through <see cref="Types"/> defined in this type and save them, along with
        /// <see cref="TypeNodeNestable"/> info.
        /// </summary>
        /// <param name="namespaces"></param>
        /// <param name="nestables"></param>
        public override void Save(Stack<string> namespaces, Stack<string> nestables)
        {
            nestables.Push(GetName());

            base.WriteInfo(namespaces, nestables);            

            foreach (var type in Types)
                type.Value.Save(namespaces, nestables);

            nestables.Pop();
        }

        public TypeMember<TypeInfo, Documentation> FindType(ArraySegment<string> types)
        {
            // Base case for when we have finally found the desired type
            if (types.Count == 1)
                return Types[types[0]].Member;

            return ((TypeNodeNestable)Types[types[0]]).FindType(types[1..]);
        }
    }
}
