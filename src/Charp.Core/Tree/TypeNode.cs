using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using System.Text.Json;
using Charp.Core.Models.Types;
using LoxSmoke.DocXml;

namespace Charp.Core.Tree
{
    public class TypeNode : Node
    {        
        public TypeMember<TypeInfo, TypeComments> Member { get; private set; }

        public TypeNode(Node parent, TypeMember<TypeInfo, TypeComments> member) : base(parent)
        {
            Member = member;
        }

        public override string GetName()
            => Member.Name;

        public override void Save(string outputPath, Stack<string> namespaces, Stack<string> nestables)
        {
            nestables.Push(GetName());
            // Combine namespace dir and nestables name convention for valid file location
            WriteInfo(outputPath, namespaces, nestables);
            nestables.Pop();
        }

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
