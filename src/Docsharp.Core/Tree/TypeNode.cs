using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using System.Text.Json;

using Docsharp.Core.Models;
using Docsharp.Core.Models.Docs;

namespace Docsharp.Core.Tree
{
    public class TypeNode : Node
    {        
        public TypeMember<TypeInfo, Documentation> Member { get; private set; }

        public TypeNode(Node parent, TypeMember<TypeInfo, Documentation> member) : base(parent)
        {
            Member = member;
        }

        public override string GetName()
            => Member.Name;

        public override void Save(Stack<string> namespaces, Stack<string> nestables)
        {
            nestables.Push(GetName());
            SaveMemberInfo(namespaces, nestables);
            nestables.Pop();
        }

        protected void SaveMemberInfo(Stack<string> namespaces, Stack<string> nestables)
        {
            string memStr = JsonSerializer.Serialize(Member);
            var test = JoinNamespaces(namespaces);
            using StreamWriter writer = new(Path.Join(JoinNamespaces(namespaces), JoinNestables(nestables)) + ".json", false);
            writer.Write(memStr);
        }
    }
}
