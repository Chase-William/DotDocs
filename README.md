## How to Use

Its simple, either provide a path to the location of the repository on your local machine or provide a url to the repository's github.

```cs
// var builder = Builder.FromPath("...");
// OR
var builder = Builder.FromUrl("...");
builder.Prepare();
builder.Build();
builder.Document();
```

#### Prepare();

Downloads the repository via the given url if nessessary and ensures the validity of the path to the new or existing repository.

#### Build();

Collects metadata, issues the MSBuild system to compile the root project and its dependencies, and instanciates models. 

#### Document();

Uses models to generate markdown files for each corresponding type and its members.

## Ways to Represent a `System.Type`

Types are organized into two different, but related concrete types:

- `TypeModel`, Represents exported types and their exported members declared in the user's projects.
- `RefTypeModel`, Represents dependency types exported from other libraries the user's types depend on *(think of ref assemblies)*.

Both the `TypeModel` and `RefTypeModel` implement the `ITypeable` interface.

### `ITypeable`

Contracts implementation of members common to both `TypeModel` and `RefTypeModel` types while also providing default functionality.

## Generics

Describes how generics and its forms are handled.

### Generic Type Parameters/Arguments

- Generic type parameters always reside within the entity that defined them.
- Non-concrete and/or constructed type arguments always reside within the entity that defined them.
- Arguments that have a concrete type e.g. *(int, string, ClassB)* reference an existing `ITypeable`.

### Generic Classes/Interfaces/Structs

Implementation details of how classes with type parameters and/or arguments are handled:

- **Pure Generic** `Map<K, V>`, if exported and declared in the user's project *(within its assembly)*, `TypeModel` type is used, however, if used as a external dependency, `RefTypeModel` is used.
- **Constructed** `Map<string, int>` or `Map<K, int>`, always uses a `RefTypeModel` and is mainly used to located the original purely generic form.

### Generic Methods

- Exported, declared, and non-constructed methods within a exported and declared type within the user's project are represented via `MethodModel`.

> Method definitions within other methods are implementation details and therefore are not within universe of discourse.

### About Generic Type Parameters/Arguments

```cs
// Generic Type Declaration
class Parent<T, K>
// Child generic type declaration and declaration of constructed parent type
class Child<T> : Parent<T, int>
```

- `Parent<T, K>`, Is a generic type & definition with type parameters visible within `System.Reflection.TypeInfo.GenericTypeParameters` property.
- `Parent<T, int>`, Is a generic type & constructed type *(is NOT a generic type definition)* with type arguments visible within `System.Type.GenericTypeArguments`. This type was created from the generic type definition `Parent<T, K>`.
- `Child<T>`, Is a generic type & definition similar to `Parent<T, K>`.

In this example, `Child` `T`'s type will be a shared reference with the constructed `Parent<T, K>`, however not with the generic type definition `Parent<T, K>`. This can be checked by inspecting the `System.Reflection.Meta.GenericParameterHandle` and `System.Reflection.MemberInfo.MetadataToken` values of type `T` between the 3 types.

> The `int` type argument within `Parent<T, int>` is a reference to the `System.Int32` type and therefore, doesn't contain information related to its use a type argument in this context unlike `T`. 

#### Nested Class Declaration and Type Parameters

```cs
class Outer<T> {
    class Inner { }
}
```

Type `Inner` **always** receives a copy of its declaring `Outer`'s generic type parameters. This can checked by inspecting the `System.Reflection.Meta.GenericParameterHandle` property of `T` from both classes showing different handles. Furthermore, it can be checked by examining the `System.Reflection.MemberInfo.MetadataToken` property.

> Had `Inner` defined generic type parameters, instead of `Outer`. `Outer` would not receive a copy and would be unable to reference `Inner`'s generic type parameters.

In the example below, *MyProperty*'s type `T` does not reference `Outer`'s `T`, but rather `Inner`'s copy of `T` from `Outer`.

> Generic type parameter `T` is copied to `Inner` even when `Inner` does not use it.

```cs
class Outer<T> {
    class Inner { 
        public T MyProperty { get; set; }
    }
}
```

#### Summary

Generic type parameters and arguments are treated differently depending on the context. 

1. In an inner/outer class structure, the type parameters are copied from the container to the contained even if the contained does not explicity use them. 
2. In a more typical approach when passing generic type parameters as arguments from one type to another, a shared reference will be created.
3. A constructed type will only contain generic type arguments.
4. If a type argments are provided to a type or method, it is marked as constructed.