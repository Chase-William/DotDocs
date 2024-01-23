using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models.Language
{
    public interface ITypeable
    {
        //public string FullName { get; }
        public string Name { get; }
        public string Namespace { get; }        

        public static ITypeable GetOrCreateTypeFrom(
            Type type,
            ImmutableDictionary<string, AssemblyModel> assemblies,
            Dictionary<string, ITypeable> types
            ) {            

            var key = type.GetUniqueName();
            // Get a ref to an existing type if exists
            if (types.ContainsKey(key))
                return types[key];
            // Create new ITypeable as one doesn't already exist

            // var handle = type.TypeHandle;

            // Concrete options are TypeModel or RefTypeModel
            // Can see if type's assembly is any of the user's assemblies
            ITypeable typeModel;
            if (assemblies.ContainsKey(type.Assembly.FullName) && !type.IsGenericParameter)
                // User defined type, therefore create TypeModel for indepth information
                typeModel = new TypeModel(type, assemblies, types);
            else
                typeModel = new RefTypeModel(type, assemblies, types);
            return typeModel;
        }

        public static void TestingTypeParams(
            Type type,
            ImmutableDictionary<string, AssemblyModel> assemblies,
            Dictionary<string, ITypeable> types
            ) {

            // Check if this type is built with type arguments
            if ((type.IsConstructedGenericType || type.IsGenericType) && type.BaseType.Name != "MulticastDelegate")
            {
                // May need to add more checks when Element types are here as they will likely be constructed types
                Console.WriteLine();
                // ConstructedTypes only have type arguments
                // Non-constructed, but generic only have type parameters
                //if (type.ContainsGenericParameters)
                //{
                //    var info = type.GetTypeInfo();
                //    for (int i = 0; i < info.GenericTypeParameters.Length; i++)
                //    {

                //    }
                //}
                //else
                //{
                //    for (int i = 0; i < type.GenericTypeArguments.Length; i++)
                //    {

                //    }
                //}               
            }
        }        
    }
}
