using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown.Extensions
{
    /// <summary>
    /// A static class containing extension methods to be used when querying <see cref="Type"/> members.
    /// </summary>
    public static class FilterExtensions
    {
        const BindingFlags DEFAULT_SEARCH = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;

        public static IEnumerable<FieldInfo> GetFieldsForEnumRendering(this Type type)
        {
            if (!type.IsEnum)
                throw new Exception($"Method {nameof(GetFieldsForEnumRendering)} was called on non enum type {type.FullName}, use {nameof(GetFieldsForTypeRendering)} instead.");

            // Avoid the value__ field generated for enums
            return type.GetFields().Where(m => !m.Attributes.HasFlag(FieldAttributes.SpecialName));
        }

        public static IEnumerable<FieldInfo> GetFieldsForTypeRendering(this Type type)
        {
            if (type.IsEnum)
                throw new Exception($"Method {nameof(GetFieldsForTypeRendering)} was called on enum {type.FullName}, use {nameof(GetFieldsForEnumRendering)} instead.");

            // Avoid compiler generate fields for backing properties
            return type.GetFields(DEFAULT_SEARCH).Where(m => !m.Attributes.HasFlag(FieldAttributes.SpecialName));
        }

        public static IEnumerable<MethodInfo> GetMethodsForRendering(this Type type)
            // Avoid backing property setter/getters amongst others
            => type.GetMethods(DEFAULT_SEARCH).Where(m => !m.Attributes.HasFlag(MethodAttributes.SpecialName));

        public static IEnumerable<PropertyInfo> GetPropertiesForRendering(this Type type)
        {
            // Using GetRuntimeProperties as properties with *private get* and *public set* are deemed private even though one could set the value from a another assembly (sound public enough to me)
            return type.GetRuntimeProperties();
        }

        public static IEnumerable<EventInfo> GetEventsForRendering(this Type type)
            => type.GetEvents(DEFAULT_SEARCH);
    }
}
