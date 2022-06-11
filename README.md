[ ] - Documentation from `.xml` needs to be assigned to each type in nodes within ModelTree.

[ ] - Documentation from `.xml` needs to be assigned to each respective member within the containing type.

## `Class` Meta

| Member | Type | Description |
| ------ | ---- | ----------- |
| IsPublic | `bool` | ... |
| IsSealed | `bool` | ... |
| IsAbstract | `bool` | ... |
| IsStatic | `bool` | ... |
| Parent | `...` | ... |

// List of interfaces it implements?
// Parent class?

## `Struct` Meta

## `Interface` Meta

### Unit Test Structure

#### Types

- `Class`
  - IsPublic
  - IsAbstract
  - IsInternal
  - IsSealed
  - IsStatic
  - Parent
- `Struct`
  - IsPublic
  - IsInternal
  - Parent
- `Interface`
  - IsPublic
  - IsInternal
- `Delegate`
  - IsPublic
  - IsInternal
  - Parent

#### Members

- `Event`
  - IsPublic
  - IsVirtual
  - IsAbstract
  - IsStatic
- `Field`
  - IsPublic
  - IsReadonly
  - IsConstant
  - IsStatic
- `Property`
  - CanGet
  - CanSet
  - IsSetPrivate
  - IsGetPrivate
  - IsAbstract
  - IsVirtual
  - IsStatic
- `Method`
  - IsPublic
  - IsAbstract
  - IsVirtual
  - IsStatic
  - ReturnType
  - Parameters