<p align="center">
  <img src="./docs/media/charp-core-logo.png" style="width: 350px;margin-left: auto;margin-right: auto;">
</p>

# Charp

Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. 

#### Types

- `Class`
  - IsPublic
  - IsPrivate
  - IsInternal
  - IsProtected
  - IsAbstract
  - IsSealed
  - IsStatic
  - Parent
- `Struct`
  - IsPublic
  - IsPrivate
  - IsInternal
  - IsProtected
  - Parent
- `Interface`
  - IsPublic
  - IsPrivate
  - IsInternal
  - IsProtected
- `Delegate`
  - IsPublic
  - IsPrivate
  - IsInternal
  - IsProtected
  - Parent

#### Members

- `Event`
  - IsPublic
  - IsStatic
  - IsProtected
  - IsInternal
  - IsAbstract
  - IsVirtual
- `Field`
  - IsPublic
  - IsReadonly
  - IsConstant
  - IsStatic
  - IsProtected
  - IsInternal
- `Property`
  - HasGetter
  - HasSetter
  - IsGetPublic
  - IsSetPublic
  - IsGetProtected
  - IsSetProtected
  - IsGetInternal
  - IsSetInternal
  - IsAbstract
  - IsVirtual
  - IsStatic
  - IsPublic
  - IsProtected
  - IsInternal
- `Method`
  - IsPublic
  - IsAbstract
  - IsVirtual
  - IsStatic
  - ReturnType
  - Parameters
  - IsInternal
  - IsProtected

### Needs Test

- `Enum` type


## Update in Charp Node

- Class HasSetter
- Class HasGetter


## Binlog Notes

### IncludeTransitiveProjectReferences

This target allows the identification of which dependencies are transitive.

### ResolveProjectReferences

This target allows the idenficiation of which dependencies are based off local .csproj files. This is important because I can scope my documentation generation to only these assemblies. Therefore, all other sources of data can be classified as external and out of scope. Documentation for this external data will only have a comment for example and a link to another site where that documentation is located.

### CoreCompile

This target contains the directory to the .xml files and the .dll.

