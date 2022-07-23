using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using System.Text.Json;
using Docshark.Core.Models.Lang.Types;
using LoxSmoke.DocXml;

namespace Docshark.Core.Tree
{
    /// <summary>
    /// A class that represents a defined type.
    /// </summary>
    public class TypeNode : Node
    {        
        /// <summary>
        /// Information about the type this <see cref="TypeNode"/> represents.
        /// </summary>
        public TypeMember<TypeInfo, TypeComments> Member { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="TypeNode"/>.
        /// </summary>
        /// <param name="parent">Parent node of this node.</param>
        /// <param name="member">Information about this type.</param>
        public TypeNode(Node parent, TypeMember<TypeInfo, TypeComments> member) : base(parent)
            => Member = member;                    

        /// <summary>
        /// Gets the short name of the current type.
        /// </summary>
        /// <returns>Short name of type.</returns>
        public override string GetName()
            => Member.Name;

        /// <summary>
        /// Modifies the provided traces while making a call to <see cref="WriteInfo(string, Stack{string}, Stack{string})"/> for writing information as JSON to file.
        /// </summary>
        /// <param name="outputPath">Location for JSON output.</param>
        /// <param name="namespaces">Namespace trace.</param>
        /// <param name="nestables">Nestable trace.</param>
        public override void Save(string outputPath, Stack<string> namespaces, Stack<string> nestables)
        {
            nestables.Push(GetName());
            // Combine namespace dir and nestables name convention for valid file location
            WriteInfo(outputPath, namespaces, nestables);
            nestables.Pop();
        }

        /// <summary>
        /// Writes all information regarding this type to file as JSON.
        /// </summary>
        /// <param name="outputPath">Location for JSON output.</param>
        /// <param name="namespaces">Namespace trace.</param>
        /// <param name="nestables">Nestable trace.</param>
        protected virtual void WriteInfo(string outputPath, Stack<string> namespaces, Stack<string> nestables)
        {
            using StreamWriter writer = new(Path.Combine(outputPath, JoinNamespaces(namespaces), JoinNestables(nestables)) + ".json", false);
            string info = string.Empty;
            switch (Member.Type)
            {
                case ClassModel.CLASS_TYPE_STRING:
                    info = JsonSerializer.Serialize(Member as ClassModel);
                    break;
                case StructModel.STRUCT_TYPE_STRING:
                    info = JsonSerializer.Serialize(Member as StructModel);
                    break;
                case InterfaceModel.INTERFACE_TYPE_STRING:
                    info = JsonSerializer.Serialize(Member as InterfaceModel);
                    break;
                case DelegateModel.DELEGATE_TYPE_STRING:
                    info = JsonSerializer.Serialize(Member as DelegateModel);
                    break;
                case EnumModel.ENUM_TYPE_STRING:
                    info = JsonSerializer.Serialize(Member as EnumModel);
                    break;
                default:
                    return;
            }
            writer.Write(info);
        }
    }
}
