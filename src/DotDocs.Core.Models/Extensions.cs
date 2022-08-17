using DotDocs.Core.Models.Exceptions;
using DotDocs.Core.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    }
}
