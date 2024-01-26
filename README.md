# DotDocs

## How to Use

Its simple, either provide a path to the location of the repository on your local machine or provide a url to the repository's github.

```cs
// Initialize
var builder = Builder.FromPath(
    @"C:\Dev\ex\DotDocs.TestData",
    new MarkdownRenderer(
        new TextFileOutput("docs", new FlatRouter(), ".md")
        )
    );

// Use
builder.Prepare();
builder.Build();
builder.Document();
```

### Prepare();

Downloads the repository via the given url if nessessary and ensures the validity of the path to the new or existing repository.

### Build();

Collects metadata, issues the MSBuild system to compile the root project and its dependencies, and instanciates models. 

### Document();

Uses models to generate markdown files for each corresponding type and its members.

## About Markdown Rendering

Rendering is done on a per file basis where each type is rendered to its own file synchronously. Moreover, the rendering pipeline behaves much like a state machine utilizing extension methods and static data to improve developer experience. This allows many referals to the `StringBuilder` and others to be hidden under the hood. However, this approach does present challenges if attempts are made to go async.

> It is typically expected for rendered content to leave adequate line height padding as courtesy to those below.

### `String` Extension Method Conventions

- **Put***MethodNameHere*, these methods write directly to the `StringBuffer`
- **As***MethodNameHere*, these methods return a modified copy of the given `string`

#### **Put** Example

```cs
"Some Text".Put();
// Writes "Some Text" to the buffer
```

The backing `StringBuffer` is statically accessible to all *Put* based methods and therefore, write directly to it.

#### **As** Example

```cs
var italicStr = "Hello".AsItalic();
// Results in: *Hello*
```

**Generally, moving away from this approach and writing directly to the buffer is ideal as to reduce allocations.**

### Type Member Render Setup

Rendering the `FieldInfo`, `MethodInfo`, `PropertyInfo`, and `EventInfo` belonging to `System.Type` is done using a setup as shown below:

```cs
// type: a local variable of a System.Type
// In this case, we're querying the fields of type
type.GetFieldsForRendering().ToMarkdown(
    before: delegate {
        // Render section header
    }, 
    // Runs for each member found
    // m: a System.Reflection.FieldInfo instance
    each: m => {
        // Render field type, name, etc.
        // Render comments
    },
    after: delegate {
        // Render section closing remarks
    });
```

This setup has proven to be flexible, yet provides appropriate structure and code re-use. It also makes great use of callbacks as defined below:

1. *before* `Action`, runs once and only once before all other callbacks
2. *each* `Action<T>` *where* `T` : `MemberInfo`, runs once for every member in the query results and a reference is accessible in the params *(m)*
3. *after* `Action`, runs once after all other callbacks completed

> If the query result return an empty collection, rendering is skipped *i.e. (before, each, and after callbacks).*