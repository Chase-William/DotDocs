using System;
using System.Collections.Generic;
using System.Reflection;

using Docsharp.Core.Models;
using Docsharp.Core.Models.Docs;

namespace Docsharp.Core.Tree
{
    /// <summary>
    /// Represents a type that can contain other type definitions internally.
    /// For example, this can represent a class or struct as both can contain other class/struct type definitions.
    /// </summary>
    public interface ITypeNodeNestable
    {
        Dictionary<string, TypeNode> Types { get; set; }

        void AddType(ArraySegment<string> types, TypeMember<TypeInfo, Documentation> member);

        TypeMember<TypeInfo, Documentation> FindType(ArraySegment<string> segments);
    }
}
