namespace Docshark.Core.TypeMapper
{
    public class TypeMap
    {
        public Dictionary<string, TypeDefinition> Types { get; } = new();

        public void AddType(Type info)
        {
            // Can return because this is the root call of this method and this type is already added
            if (Types.ContainsKey(info.ToString()))
                return;                 
            AddTypeRecursive(info);
        }

        void AddTypeRecursive(Type info, string? parentId = null)
        {
            // The current type definition instance for the given type info
            TypeDefinition type;
            /**
             * Create a new type definition
             * Add the type defintion to the dictionary of types
             * If it has a BaseType (Parent) call this function for that type (recursively)                         
             * This will flush out all other types the current type definition depends on
             */
            if (!Types.ContainsKey(info.ToString()))
            {
                type = TypeDefinition.From(info);
                Types.Add(info.ToString(), type);
                
                // Process all type dependencies recursively for this type
                if (info.BaseType != null)                
                    AddTypeRecursive(info.BaseType, info.BaseType.ToString());                
            }
            else // Get a ref to the existing type definition     
                type = Types[info.ToString()];            
            
            var parameters = info.GenericTypeArguments;
            // If this type uses type arguments ...<...> process those recursively too
            if (parameters.Length > 0)
                AddTypeArgumentsRecursive(parameters, type);
        }

        void AddTypeArgumentsRecursive(Type[] parameters, TypeDefinition type)
        {
            // Process all type parameters defined
            foreach (var param in parameters)
            {
                // Ensure each one and it's dependencies are accounted for
                AddTypeRecursive(param, type.Id);
                // Once accounted for, add the type to the list of arguments for the given type
                type.TypeArguments.Add(Types[param.ToString()].Id);
            }
        }
    }
}
