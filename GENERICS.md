## How Generics Work *(Using Reflection)*


### Generic Type Definition

A *Generic Type Definition* is the most polymorphic version of a type created. This type will create *Constructed Generic Types* when other types derive from it and provide type arguments.

A `System.Type` instance denotes if the type is a *Generic Type Defintion* via the *IsGenericTypeDefinition* property.

```cs
class GenericClass<T> { }
```

### Constructed Generic Type

A *Constructed Generic Type* is a concrete instance of a more polymorphic version of the same type. For instance, in the example below look at the parent for `MyClass`. Behind the scenes that `GenericClass<SomeArgument>` is it's own type and is marked as a *Constructed Generic Type*.

A `System.Type` instance denotes if a type is a *Constructed Generic Type* via the *IsConstructedGenericType* property.

```cs
class MyClass : GenericClass<SomeArgument> { }
```