using Docshark.Core.Global.Assemblies;
using Docshark.Core.Models;
using System.Text.Json;

namespace Docshark.Core.Global.Types
{
    /// <summary>
    /// Maps types into a dictionary designed to be serialized and usable like a database.
    /// </summary>
    public class TypeMapper : IMapper<TypeDefinition>
    {
        public const string TYPE_MAPPER_FILENAME = "types.json";

        /// <summary>
        /// Contains all <see cref="TypeDefinition"/> used in the root project and its dependencies.
        /// </summary>
        public IReadOnlyDictionary<string, TypeDefinition> MappedDefinitions => mappedDefinitions;

        Dictionary<string, TypeDefinition> mappedDefinitions = new();

        AssemblyMapper? asmMapper;

        public TypeMapper(AssemblyMapper asmMapper = null)
        {
            this.asmMapper = asmMapper;
        }        

        public void AddType(Type info)
        {
            // Can return because this is the root call of this method and this type is already added
            if (MappedDefinitions.ContainsKey(info.GetPrimaryKey()))
                return;

            //if (info.ContainsGenericParameters)
            //{
            //    return;
            //}
            AddTypeRecursive(info);
        }

        void AddTypeRecursive(Type info, string? parentId = null)
        {

            var pk = info.GetPrimaryKey();
            /*
             * Create a new type definition
             * Add the type defintion to the dictionary of types
             * If it has a BaseType (Parent) call this function for that type (recursively)                         
             * This will flush out all other types the current type definition depends on
             */
            if (!MappedDefinitions.ContainsKey(pk))
            {
                TypeDefinition type = TypeDefinition.From(info, asmMapper);
                mappedDefinitions.Add(pk, type);
                // type.IsDefinedInUserProject = CheckNamespace.Invoke(type.Namespace);
                if (parentId == null)
                    parentId = type.BaseType;
                // Process all type dependencies recursively for this type
                if (info.BaseType != null)                
                    AddTypeRecursive(info.BaseType, info.BaseType.GetPrimaryKey());

                var parameters = info.GenericTypeArguments;
                // If this type uses type arguments ...<...> process those recursively too
                if (parameters.Length > 0)
                    AddTypeArgumentsRecursive(parameters, type);
            }                    
        }

        void AddTypeArgumentsRecursive(Type[] parameters, TypeDefinition type)
        {            
            // Process all type parameters defined
            foreach (var param in parameters)
            {
                // Generic type params like T1 & T2 need to be handled differently
                // Therefore, implement them here
                // Generic type arguments are defined in the typeArgument list so they're scoped to the class,
                // whereas non-generic type arguments are defined in the global scope
                if (param.ContainsGenericParameters)
                {
                    // As of rule of thumb, generic type parameters cannot have their own type parameters..
                    // Therefore no recursion is needed, simply add define this always new argument to the
                    // typeArguments and continue...
                    type.TypeArguments.Add(new GenericTypeDefinition
                    {
                        Name = param.GetPrimaryKey(),
                        BaseType = TypeKey.From(param.BaseType)
                    });
                }
                else // Implement non-generic type arguments below
                {
                    // Ensure each one and it's dependencies are accounted for
                    AddTypeRecursive(param, type.GetPrimaryKey());
                    // Once accounted for, add the type to the list of arguments for the given type
                    type.TypeArguments.Add(MappedDefinitions[param.GetPrimaryKey()].GetPrimaryKey());
                }                
            }
        }
    }
}
