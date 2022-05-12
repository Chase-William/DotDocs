using System.Reflection;

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
    }
}
