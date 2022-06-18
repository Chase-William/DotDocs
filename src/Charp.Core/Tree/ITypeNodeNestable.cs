using System;
using System.Collections.Generic;
using System.Reflection;

using Charp.Core.Models;
using Charp.Core.Models.Types;
using LoxSmoke.DocXml;

namespace Charp.Core.Tree
{
    /// <summary>
    /// Represents a type that can contain other type definitions internally.
    /// For example, this can represent a class or struct as both can contain other class/struct type definitions.
    /// </summary>
    public interface ITypeNodeNestable
    {
        /// <summary>
        /// Contains the types defined within the implementing type.
        /// </summary>
        Dictionary<string, TypeNode> Types { get; set; }
        /// <summary>
        /// Adds a given type with it's member info.
        /// </summary>
        /// <param name="fullName">Fullname of type to add.</param>
        /// <param name="member">Metadata to be associated with the type.</param>
        void AddType(ArraySegment<string> fullName, TypeMember<TypeInfo, TypeComments> member);
        /// <summary>
        /// Finds the given type.
        /// </summary>
        /// <param name="segments">Collection of namespaces and types leading to where the target type.</param>
        /// <returns>Metadata about the target type.</returns>
        TypeMember<TypeInfo, TypeComments> FindType(ArraySegment<string> segments);
        /// <summary>
        /// Finds a given field within a type.
        /// </summary>
        /// <param name="segments">Collection of namespaces and types leading to the target field.</param>
        /// <returns>Metadata about the target field.</returns>
        Model<FieldInfo, CommonComments> FindField(ArraySegment<string> segments);
        /// <summary>
        /// Finds a given property within a type.
        /// </summary>
        /// <param name="segments">Collection of namespaces and types leading to the target property.</param>
        /// <returns>Metadata about the target property.</returns>
        Model<PropertyInfo, CommonComments> FindProperty(ArraySegment<string> segments);
        /// <summary>
        /// Finds a given event within a type.
        /// </summary>
        /// <param name="segments">Collection of namespaces and types leading to the target event.</param>
        /// <returns>Metadata about the target property.</returns>
        Model<EventInfo, CommonComments> FindEvent(ArraySegment<string> segments);
    }
}
