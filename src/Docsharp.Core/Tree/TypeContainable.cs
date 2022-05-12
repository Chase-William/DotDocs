using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Docsharp.Core.Models;
using Docsharp.Core.Models.Docs;

namespace Docsharp.Core.Tree
{
    public class TypeContainable : TypeNode, ITypeContainable
    {
        public Dictionary<string, Node> Types { get; set; } = new();

        public TypeContainable(Node parent, TypeMember<TypeInfo, Documentation> member) : base(parent, member)
        { }

        public void AddType(ArraySegment<string> types, TypeMember<TypeInfo, Documentation> member)
        {
            string typeName = types.First();
            // End of nested type chain reached
            if (types.Count == 1)
            {
                if (member.CanHaveInternalTypes)
                {
                    Types.Add(typeName, new TypeContainable(this, member));
                    return;
                }
                Types.Add(typeName, new TypeNode(this, member));
                return;
            }

            TypeContainable type;
            if (!Types.ContainsKey(typeName))
            {
                type = new TypeContainable(this, null);
                Types.Add(typeName, type);
            }
            else
                type = (TypeContainable)Types[typeName];
            types = types[1..types.Count];
            type.AddType(types, member);
        }
    }
}
