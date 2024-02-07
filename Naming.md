# Fetching Names

We need to check the following recursively on each type:

1. Arguments
2. Parameters
3. ElementTypes

### Examples

#### `int`

Just print the root name.

#### `Generic<int, long>`

Print root name and print args.

#### `Generic<int, T>`

Print root name and print args.

#### `class` `Generic<T, K>`

In this class definition, print root name and parameters.



```cs

public static class TypeNaming {
    static Type origin;

    // Public client API
    public static void PutTypeName(this Type to, Type? from = null) {
        origin = from; // Store origin state for routing
        to.PutTypeName(); // Begin recursively retrieving type names
    }

    static void PutTypeName(this Type to) {
        // If array, look into and get element type
        if (to.IsArray) {
            to = type.GetElementType();
        }
        else { // Not array, put root name
            to.PutMaybeLink();
        }

        // If type is generic, there must be generic type params or args present, otherwise skip
        if (type.IsGenericType)
        {
            if (type.GenericTypeArguments.Any()) // Process args if they exits
                type.GenericTypeArguments.PutTypeArgs();
            else // Args didn't exists, therefore, params must
                type.GetTypeInfo().GenericTypeParameters.PutTypeParams();
        } 

        // When returning back up the call chain, if this was an array, put the suffix
        if (type.IsArray) {
            "[]".Put()
        }
    }

    static void PutTypeArgs(this IEnumerable<Type> args) {
        // - Type args can take type arguments
        // - Type args can take an element type
        // - Type args may be linkable to a user defined type
        // Therefore, call back recursively
        foreach (var arg in args)
            arg.PutTypeName();
    }

    static void PutTypeParams(this IEnumerable<Type> _params) {
        // - Type Params cannot take type arguments
        // - Type Params cannot take an element type
        // - Type Params will never be linkable (their defined in a type's declaration)
        // Therefore, just the type as a code span
        foreach (var param in _params)
            AsMarkdown.Code.Wrap(param.Name)
    }

    static void PutMaybeLink(this Type to) {
        string name;

        // Remove arg/param count label from type's name if generic
        if (to.IsGenericType)
            name = new string(type.Name.TakeWhile(static c => c != '`').ToArray());        

        // Check other states of "to" and "origin" for linking validity      
        if (origin is not null &&
            !to.IsGenericParameter && // Cannot be a generic type parameter (dest wont exists)
            // !origin.IsGenericParameter && // ^
            to.Name != origin.Name && // Do not link to one's self
            State.Assemblies.ContainsKey(to.Assembly.FullName)) // Ensure this type was defined in the user's projects otherwise a destination won't exist
        {
            // Render link
            var path = State.Output.Router.GetRoute(from, to);
            AsMarkdown.Link.Link(name, path, AsMarkdown.Code, padding);
            return;
        }
        // Render normal code span for type representation
        AsMarkdown.Code.Wrap(name, padding);
    }
}
```