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
    public static class Extensions
    {
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

        public static string GetAssemblyId(this Assembly assembly)
        {
            var name = assembly.GetName();
            if (name.Name == null)
                throw new RequiredAssemblyPropertyNullException(assembly, nameof(name.Name));
            return name.Name;
        }

        public static string GetProjectId(this LocalProjectModel project)
            => project.ProjectName;

        public static IEnumerable<PropertyInfo> GetDesiredProperties(this Type type)
            => type.GetRuntimeProperties();

        public static IEnumerable<MethodInfo> GetDesiredMethods(this Type type)
            => type.GetRuntimeMethods()
                   .Where(method => !method.IsSpecialName && !typeof(object).GetRuntimeMethods().Any(name => name.Equals(method.Name)));

        public static IEnumerable<EventInfo> GetDesiredEvents(this Type type)
            => type.GetRuntimeEvents();

        public static IEnumerable<FieldInfo> GetDesiredFields(this Type type)
            => type.GetRuntimeFields()
                   .Where(_field => !_field
                       .GetCustomAttributesData()
                       .Any(attr => attr.AttributeType.Name == typeof(CompilerGeneratedAttribute).Name) &&
                   !_field.Attributes.HasFlag(FieldAttributes.SpecialName) &&
                   !_field.Attributes.HasFlag(FieldAttributes.RTSpecialName));

        public static IEnumerable<FieldInfo> GetEnumDesiredFields(this Type type)
        {
            if (!type.IsEnum)
                throw new ArgumentException($"The provided type {type.FullName} cannot be used because it is not an enumeration.");
            return type.GetRuntimeFields()
                   .Where(_field => !_field
                       .GetCustomAttributesData()
                       .Any(attr => attr.AttributeType.Name == typeof(CompilerGeneratedAttribute).Name));
        }     
    }
}
