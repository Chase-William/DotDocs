using Charp.Core.Models;
using Charp.Core.Models.Types;
using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Charp.Core.Tree
{
    public class TypeNodeNestable : TypeNode, ITypeNodeNestable
    {
        public Dictionary<string, TypeNode> Types { get; set; } = new();

        public TypeNodeNestable(Node parent, TypeMember<TypeInfo, TypeComments> member) : base(parent, member)
        { }

        public void AddType(ArraySegment<string> types, TypeMember<TypeInfo, TypeComments> member)
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
        public override void Save(string outputPath, Stack<string> namespaces, Stack<string> nestables)
        {
            nestables.Push(GetName());

            base.WriteInfo(outputPath, namespaces, nestables);            

            foreach (var type in Types)
                type.Value.Save(outputPath, namespaces, nestables);

            nestables.Pop();
        }

        public TypeMember<TypeInfo, TypeComments> FindType(ArraySegment<string> types)
        {            
            // Base case for when we have finally found the desired type
            if (types.Count == 1)
                return Member;
            // Check nested types
            return ((TypeNodeNestable)Types[types[0]]).FindType(types[1..]);
        }

        public Model<FieldInfo, CommonComments> FindField(ArraySegment<string> types)
        {
            // Base case for when we have finally found the desired type
            if (types.Count == 1)
                return ((IFieldable)Member).Fields.FirstOrDefault(f => f.Name.Equals(types[0]));
            // Check nested types
            return ((TypeNodeNestable)Types[types[0]]).FindField(types[1..]);
        }

        public Model<PropertyInfo, CommonComments> FindProperty(ArraySegment<string> types)
        {
            if (types.Count == 1)
                return ((IMemberContainable)Member).Properties.FirstOrDefault(f => f.Name.Equals(types[0]));
            // Check nested types
            return ((TypeNodeNestable)Types[types[0]]).FindProperty(types[1..]);
        }

        public Model<EventInfo, CommonComments> FindEvent(ArraySegment<string> types)
        {
            if (types.Count == 1)
                return ((IMemberContainable)Member).Events.FirstOrDefault(f => f.Name.Equals(types[0]));
            // Check nested types
            return ((TypeNodeNestable)Types[types[0]]).FindEvent(types[1..]);
        }
    }
}
