using DotDocs.Core.Build;
using DotDocs.Models;
using DotDocs.Models.Language;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DotDocs.Core.Util
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

        public static RepositoryModel Apply(this RepositoryModel model, Repository repo)
        {
            model.Name = repo.Name;
            model.Url = repo.Url;
            model.Commit = repo.Commit;
            // Handles creation of the project and all down stream entities
            model.Projects.Add(repo.build.MakeModels());
            return model;
        }

        public static ProjectModel Apply(this ProjectModel model, ProjectBuildInstance build)
        {
            model.Name = build.ProjectFileName;
            model.Assembly = new AssemblyModel().Apply(build.Assembly);
            return model;
        }

        public static AssemblyModel Apply(this AssemblyModel model, Assembly assembly)
        {
            model.Name = assembly.GetName().Name;

            List<TypeModel> models = new List<TypeModel>();
            foreach (var type in assembly.DefinedTypes)
            {
                models.Add(new TypeModel().Apply(type));
            }
            model.Types = models;
            return model;
        }

        public static TypeModel Apply(this TypeModel model, Type type)
        {
            model.Name = type.Name;
            
            // TODO: From here we would perform addition logic for handling member models

            return model;
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
        public static bool IsInternal(this Type info) => info.IsNotPublic && !info.IsNested || info.IsNestedAssembly || info.IsNestedFamORAssem || info.IsNestedFamANDAssem;
        public static bool IsProtected(this Type info) => info.IsNestedFamily || info.IsNestedFamANDAssem || info.IsNestedFamORAssem;       
    }
}
