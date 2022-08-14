using Docshark.Core.Global.Assemblies;
using Docshark.Core.Global.Types.Generic;
using Docshark.Core.Models;
using System.Reflection;

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
        GenericTypeMapper? genericMapper;

        public TypeMapper(GenericTypeMapper genericMapper = null, AssemblyMapper asmMapper = null)
        {
            this.genericMapper = genericMapper;
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

            if (pk == "Docshark.Core.Models.Codebase.Model\u00602[T1,T2]")
                Console.WriteLine();

            // Always prevent generic types from being added to the normal type list
            if (info.IsGenericParameter)
            {
                // Add the type to the generic type list if needed
                if (!genericMapper.MappedDefinitions.ContainsKey(pk))
                    genericMapper.Add(info);
                return;
            }



            /*
             * Create a new type definition
             * Add the type defintion to the dictionary of types
             * If it has a BaseType (Parent) call this function for that type (recursively)                         
             * This will flush out all other types the current type definition depends on
             */
            if (!MappedDefinitions.ContainsKey(pk))
            {              
                if (pk == "T=Docshark.Core.Global.Definition")
                    Console.WriteLine();
                TypeDefinition type = TypeDefinition.From(info, asmMapper);
                mappedDefinitions.Add(pk, type);
                // type.IsDefinedInUserProject = CheckNamespace.Invoke(type.Namespace);
                if (parentId == null)
                    parentId = type.BaseType;
                // Process all type dependencies recursively for this type
                if (info.BaseType != null)                
                    AddTypeRecursive(info.BaseType, info.BaseType.GetPrimaryKey());
                
                if (info.ContainsGenericParameters)
                {
                    var meta = info.GetTypeInfo();
                    AddTypeParameters(meta, type);
                }

                var args = info.GenericTypeArguments;
                // If this type uses type arguments ...<...> process those recursively too
                if (args.Length > 0)
                    AddTypeArgumentsRecursive(args, type);
            }                    
        }

        void AddTypeParameters(TypeInfo info, TypeDefinition type)
        {
            foreach (var param in info.GenericTypeParameters)
            {
                AddTypeRecursive(param, type.GetPrimaryKey());
                // Generic type params like T1 & T2 need to be handled differently
                // Therefore, implement them here
                // Generic type arguments are defined in the typeArgument list so they're scoped to the class,
                // whereas non-generic type arguments are defined in the global scope

                // As of rule of thumb, generic type parameters cannot have their own type parameters..
                // Therefore no recursion is needed, simply add define this always new argument to the
                // typeArguments and continue...                    
                type.TypeParameters.Add(param.GetPrimaryKey());
            }
        }

        void AddTypeArgumentsRecursive(Type[] parameters, TypeDefinition type)
        {            
            // Process all type parameters defined
            foreach (var param in parameters)
            {                
                // Ensure each one and it's dependencies are accounted for
                AddTypeRecursive(param, type.GetPrimaryKey());
                // Once accounted for, add the type to the list of arguments for the given type
                // var def = MappedDefinitions[param.GetPrimaryKey()];
                if (param.IsGenericType)
                    type.TypeParameters.Add(param.GetPrimaryKey());
                else
                    type.TypeArguments.Add(param.GetPrimaryKey());                     
            }
        }
    }
}
