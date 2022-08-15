using Docshark.Core.Global.Assemblies;
using Docshark.Core.Global.Parameters;
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
        GenericParameterMapper? genericMapper;

        public TypeMapper(GenericParameterMapper genericMapper = null, AssemblyMapper asmMapper = null)
        {
            this.genericMapper = genericMapper;
            this.asmMapper = asmMapper;
        }        

        public void AddType(Type info)
        {
            // Can return because this is the root call of this method and this type is already added
            if (MappedDefinitions.ContainsKey(info.GetPrimaryKey()))
                return;

            AddTypeRecursive(info);
        }

        void AddTypeRecursive(Type info, string? parentId = null)
        {
            var pk = info.GetPrimaryKey();                    

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
                // Ensure the base type of this type is accounted for
                if (param.BaseType != null)
                    AddTypeRecursive(param.BaseType, type.GetPrimaryKey());           
                type.TypeParameters.Add(TypeKey.From(param));
            }
        }

        void AddTypeArgumentsRecursive(Type[] arguments, TypeDefinition type)
        {            
            // Process all type parameters defined
            foreach (var arg in arguments)
            {                
                // Ensure each one and it's dependencies are accounted for
                AddTypeRecursive(arg, type.GetPrimaryKey());
                type.TypeArguments.Add(TypeKey.From(arg));                     
            }
        }
    }
}
