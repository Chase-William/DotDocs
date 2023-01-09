using DotDocs.Core.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Core.Models
{
    /// <summary>
    /// A static class that exists purely to contain extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Collection of members always present in an object.
        /// Works for structs too because they are <see cref="ValueType"/> which is a class behind the scenes.
        /// </summary>
        static readonly string[] DEFAULT_OBJECT_METHODS = typeof(object).GetRuntimeMethods().Select(m => m.Name).ToArray();

        /// <summary>
        /// Gets a unique identifier for a type.
        /// </summary>
        /// <param name="type">The type to get an id for.</param>
        /// <returns>A string that is the id.</returns>
        public static string GetTypeId(this Type type)
        {
            /*
             * The Id for a type needs to be a composite of the MetadataToken and the result of the ToString().
             * MetadataToken:
             * This is a unique identifier for each type, however is the same for constructed types from a 
             * generic type definition. Therefore, to stay unique more info is needed.
             * ToString():
             * The result of ToString provides the perfect value to discriminate between constructed types
             * and generic type definitions. It also provides a namespace to the type which isn't really needed
             * because of the MetadataToken, but is allowable regardless. ToString will also provide
             * the Name of a type if the FullName is't available (nice for generic parameters).
             */
            return type.MetadataToken + "-" + type.ToString();
        }
        /// <summary>
        /// Gets a unique idi for an assembly.
        /// </summary>
        /// <param name="assembly">The assembly to get an id for.</param>
        /// <returns>A string that is the id.</returns>
        /// <exception cref="RequiredAssemblyPropertyNullException">Occurs when an assembly doesn't have a name.</exception>
        public static string GetAssemblyId(this Assembly assembly)
        {
            var name = assembly.GetName();
            if (name.Name == null)
                throw new RequiredAssemblyPropertyNullException(assembly, nameof(name.Name));
            return name.Name;
        }

        /// <summary>
        /// Gets a list of the desired properties that DotDocs will only filter down further as needed.
        /// </summary>
        /// <param name="type">The type to get property infos from.</param>
        /// <returns>Desired properties from the type.</returns>
        public static IEnumerable<PropertyInfo> GetDesiredProperties(this Type type)
            => type.GetRuntimeProperties();
        /// <summary>
        /// Gets a list of desired methods that DotDocs will only filter down further as needed. 
        /// This method will prevent the returning of generates methods for property getter and setters.
        /// </summary>
        /// <param name="type">The type to get the method infos from.</param>
        /// <returns>Desired methods from the type.</returns>
        public static IEnumerable<MethodInfo> GetDesiredMethods(this Type type)
            => type.GetRuntimeMethods()
                   .Where(method => !method.IsSpecialName && !DEFAULT_OBJECT_METHODS.Any(name => name == method.Name));
        /// <summary>
        /// Gets a list of desired events that DotDocs will only filter down further as needed.
        /// </summary>
        /// <param name="type">The type to get the event infos from.</param>
        /// <returns>Desired events from the type.</returns>
        public static IEnumerable<EventInfo> GetDesiredEvents(this Type type)
            => type.GetRuntimeEvents();
        /// <summary>
        /// Gets a list of the desired fields that DotDocs will only filter down further as needed.
        /// This method will prevent the returning of generates fields for properties.
        /// </summary>
        /// <param name="type">The type to get the fields from.</param>
        /// <returns>Desired fields from the type.</returns>
        public static IEnumerable<FieldInfo> GetDesiredFields(this Type type)
            => type.GetRuntimeFields()
                   .Where(_field => !_field
                       .GetCustomAttributesData()
                       .Any(attr => attr.AttributeType.Name == typeof(CompilerGeneratedAttribute).Name) &&
                   !_field.Attributes.HasFlag(FieldAttributes.SpecialName) &&
                   !_field.Attributes.HasFlag(FieldAttributes.RTSpecialName));
        /// <summary>
        /// Gets a list of desired enums that DotDocs will only filter down further as needed.
        /// This methid will prevent the returning of any compiler generates fields.
        /// </summary>
        /// <param name="type">The type to get the fields from.</param>
        /// <returns>Desired fields from the type.</returns>
        /// <exception cref="ArgumentException">Raised when this method has been called on a type that is not an enum.</exception>
        public static IEnumerable<FieldInfo> GetEnumDesiredFields(this Type type)
        {
            if (!type.IsEnum)
                throw new ArgumentException($"The provided type {type.FullName} cannot be used because it is not an enumeration.");
            return type.GetRuntimeFields()
                   .Where(_field => !_field
                       .GetCustomAttributesData()
                       .Any(attr => attr.AttributeType.Name == typeof(CompilerGeneratedAttribute).Name));
        }
        public static IEnumerable<Type> GetDesiredInterfaces(this Type type)
            => type.GetInterfaces();

        // TODO: Consider this, do we really want to display public classes that could be inside a private class?
        // public static bool IsPublic(this TypeInfo info) => info.IsPublic || info.IsNestedPublic;
        public static bool IsPublic(this Type info) => info.IsPublic || info.IsNestedPublic;
        public static bool IsPrivate(this Type info) => info.IsNestedPrivate;        
        public static bool IsInternal(this Type info) => (info.IsNotPublic && !info.IsNested) || info.IsNestedAssembly || info.IsNestedFamORAssem || info.IsNestedFamANDAssem;
        public static bool IsProtected(this Type info) => info.IsNestedFamily || info.IsNestedFamANDAssem || info.IsNestedFamORAssem;           

        public static Perspective ToPerspective(this string perspective)
        {
            if (perspective == ConfigConstants.EXTERNAL_PERSPECTIVE)
                return Perspective.External;
            else if (perspective == ConfigConstants.INTERNAL_PERSPECTIVE)
                return Perspective.Internal;
            throw new ArgumentException($"The value provided of {perspective} does not align with any of the Perspective enum values.");
        }

        public static bool From(this Perspective perspective, AccessibilityModifier mod) => mod switch
            {
                AccessibilityModifier.Public => true,
                AccessibilityModifier.Protected | AccessibilityModifier.Internal when perspective == Perspective.Internal => true,
                AccessibilityModifier.Private | AccessibilityModifier.Protected when perspective == Perspective.Internal => true,
                AccessibilityModifier.Protected => true,
                AccessibilityModifier.Internal when perspective == Perspective.Internal => true,
                AccessibilityModifier.Private when perspective == Perspective.Internal => true,
                _ => false
            };
        
    }
}
