using Docshark.Core.Models.Lang;
using Docshark.Core.Models.Lang.Types;
using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Docshark.Core.Tree.Nodes
{
    /// <summary>
    /// A type that can contain other types.
    /// </summary>
    public class TypeNodeNestable : TypeNode, ITypeNodeNestable
    {
        /// <summary>
        /// Nested types.
        /// </summary>
        public Dictionary<string, TypeNode> Types { get; set; } = new();

        /// <summary>
        /// Intializes a new instance of <see cref="TypeNodeNestable"/>.
        /// </summary>
        /// <param name="parent">Parent of this node.</param>
        /// <param name="member">Information about this type.</param>
        public TypeNodeNestable(Node parent, TypeMember<TypeInfo, TypeComments> member) : base(parent, member)
        { }

        /// <summary>
        /// A recursive method for adding a type to another <see cref="TypeNodeNestable"/>.
        /// </summary>
        /// <param name="types">Type trace.</param>
        /// <param name="member">Information about the type to be added.</param>
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
        /// <param name="outputPath">Location to write to.</param>
        /// <param name="namespaces">Namespace trace.</param>
        /// <param name="nestables">Type trace.</param>
        public override void Save(string outputPath, Stack<string> namespaces, Stack<string> nestables)
        {
            nestables.Push(GetName());

            base.WriteInfo(outputPath, namespaces, nestables);

            foreach (var type in Types)
                type.Value.Save(outputPath, namespaces, nestables);

            nestables.Pop();
        }

        /// <summary>
        /// Searches through this <see cref="TypeNodeNestable"/> and it's nested <see cref="Types"/>.
        /// </summary>
        /// <param name="types">Type trace.</param>
        /// <returns>Information about the target type.</returns>
        public TypeMember<TypeInfo, TypeComments> FindType(ArraySegment<string> types)
        {
            // Base case for when we have finally found the desired type
            if (types.Count == 1)
                return Member;
            // Check nested types
            return ((TypeNodeNestable)Types[types[0]]).FindType(types[1..]);
        }

        /// <summary>
        /// Searches through this <see cref="TypeNode.Member"/> as a <see cref="IFieldable"/> to find the target field.
        /// </summary>
        /// <param name="types">Type trace.</param>
        /// <returns>Information about the target field.</returns>
        public Model<FieldInfo, CommonComments> FindField(ArraySegment<string> types)
        {
            // Base case for when we have finally found the desired type
            if (types.Count == 1)
                return ((IFieldable)Member).Fields.FirstOrDefault(f => f.Name.Equals(types[0]));
            // Check nested types
            return ((TypeNodeNestable)Types[types[0]]).FindField(types[1..]);
        }

        /// <summary>
        /// Searches through this <see cref="TypeNode.Member"/> as a <see cref="IMemberContainable"/> to find the target property.
        /// </summary>
        /// <param name="types">Type trace.</param>
        /// <returns>Information about the target property.</returns>
        public Model<PropertyInfo, CommonComments> FindProperty(ArraySegment<string> types)
        {
            if (types.Count == 1)
                return ((IMemberContainable)Member).Properties.FirstOrDefault(f => f.Name.Equals(types[0]));
            // Check nested types
            return ((TypeNodeNestable)Types[types[0]]).FindProperty(types[1..]);
        }

        /// <summary>
        /// Searches through this <see cref="TypeNode.Member"/> as a <see cref="IMemberContainable"/> to find the target event.
        /// </summary>
        /// <param name="types">Type trace.</param>
        /// <returns>Information about the target event.</returns>
        public Model<EventInfo, CommonComments> FindEvent(ArraySegment<string> types)
        {
            if (types.Count == 1)
                return ((IMemberContainable)Member).Events.FirstOrDefault(f => f.Name.Equals(types[0]));
            // Check nested types
            return ((TypeNodeNestable)Types[types[0]]).FindEvent(types[1..]);
        }
    }
}
