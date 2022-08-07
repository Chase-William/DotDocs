

## Directories

### `project/`
The `project` directory contains the current project and all it's local project dependencies. The contents of each projects folder is identical to the source besides the following:

- Namespaces always have a folder
- Each file only contains one type, therefore nested types are in their own file *(look for `<parent>+<child>` structure)*
- All files are `.json` files

### `meta/`
The `meta` directory contains all the *glue* that binds the local projects and their external dependencies together.

#### `types.json`
This file consists of many `TypeDefinition` types which connects all type information so type hierarchies can be looked up to root. This also includes type arguments and their parents as well. Other important facts:

- Only one `TypeDefinition` exists per type
- Types are linked via a key that is their *<namespace>.<type>* or *FullName*

An example type definition:

```json
"Id": "Docshark.Core.BuildManager",
"Parent": "System.Object",
"TypeArguments": [],
"Namespace": "Docshark.Core",
"AssemblyPath": "C:\\Dev\\Docshark.Core\\src\\Docshark.Core\\bin\\Debug\\net6.0\\Docshark.Core.dll",
"AssemblyName": "Docshark.Core.dll",
"TypeName": "BuildManager",
"Comments": null
```