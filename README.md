![DotDocs_logo_150x150](https://github.com/Chase-William/DotDocs/assets/46757278/ca2b9be4-b1cf-4775-bb04-7a6e5ff7db82)

# DotDocs [![Build](https://github.com/Chase-William/DotDocs/actions/workflows/build.yml/badge.svg)](https://github.com/Chase-William/DotDocs/actions/workflows/build.yml) [![Tests](https://github.com/Chase-William/DotDocs/actions/workflows/tests.yml/badge.svg)](https://github.com/Chase-William/DotDocs/actions/workflows/tests.yml)

Generate easy add hoc docs from your C# codebase in markdown.

## How to Use

![DotDocs_how_it_works](https://github.com/Chase-William/DotDocs/assets/46757278/337229ed-06f7-4d6d-ada0-9a8e11a026f0)

Its simple, provide a path to the location of the repository containing your C# projects and specify how Dotdocs shall render them.

```cs
// Initialize
var builder = Builder.FromPath(
    "<repo root directory>",
    new MarkdownRenderer(
        new TextFileOutput("<output dir>", new FlatRouter(), ".md")
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

## Comment Rendering

Render output from comments can vary depending on state of the comment itself and/or the entity it denotes. This section aims to provide clarity as to how rendering behavior varies depending on state.

- If *Summary*, *Example*, or *Remarks* are empty, they'll just skip rendering.
- A listing of *Type Parameters*, *Type Arguments*, *Parameters*, and *Returns* will always be provided agnostic to the existance of associated comments if appropriate.
- Property *get* and *set* functions are documentation similarly to *Visual Studio*'s style.

***See Examples in [formats](./formats/) folder.***

#### No Comments:
```java
// Remember this is not rendered into Markdown in the example...
- *@typeparam* `T`
- *@param* `string` ***str***
- *@param* `T` ***_value***
- *@returns* `ulong`
```

#### With Comments:

```java
- *@typeparam* `T`, Tempor incididunt ut labore et.
- *@param* `string` ***str***, Consectetur adipiscing elit, sed eiusmod tempor incididunt ut labore et dolore magna aliqua.
- *@param* `T` ***_value***, Elit, sed eiusmod tempor incididunt ut labore et dolore magna aliqua.
- *@returns* `ulong`, Adipiscing elit, sed eiusmod tempor incididunt ut labore et dolore magna aliqua.
```

The bullet listing even without comments can assist in visually parsing complex delcarations.

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

## Creating Project Dependency Graph

- The starting project can be anywhere in the graph
- The graph can contain multiple isolated clusters
- There can be multiple parent project nodes
- There can be multiple child project nodes
- Each project file is represented by a single shared instance of a node
- Friendly API method must take a collection *(data set)* of project file paths assuming each item is unique
- Friendly API method must return a collection of root project files nodes

### Impl

- Use a dictionary to keep track of all `ProjectDocument`



- Iterate over *projectFiles*
- Check if current project file has children, if so process them and adjust, otherwise next
- When a new project adds another project as a dep, ensure the dep has that project added as a parent

```cs
// Public API
public static IEnumerable<ProjectDocument> GetProjects(IEnumerable<string> projectFiles) {
  var projects = new Dictionary<string, ProjectDocument>();
  var roots = new List<ProjectDocument>();
  foreach (var location in projectFiles) {
    // If a ProjectDocument and it's dependency tree hasnt been created, create it
    if (!projects.ContainsKey(location)) {
      // Add each level 0 node returned to the list as a possible root project
      roots.Add(CreateProjectAndDependencies(location, projects));
    }
  }
  // Return all the root projects
  return roots;
}

static CreateProjectAndDependencies(string location, Dictionary<string, ProjectDocument> projects) {
  // If a ProjectDocument already exists for the current project, return it, otherwise create it and return it
  // When called recursively, this allows us to add an existing dependency tree to a newly created root node
  if (projects.TryGetValue(location, out ProjectDocument proj)) {
    return proj;
  }
  // Create ProjectDocument

  // ..

  // If this project has project dependencies, iterate over them and call this function recursively to handle them
  foreach (var depLocation in @dependencies) {
    // Get the dependency project tree using this function
    var dep = CreateProjectAndDependencies(depLocation, projects);
    // Add it to the current project's deps
    proj.Dependencies.Add(dep);    
  } // Do this for all of this projects deps

  return proj; // Return once this project tree and it's dependency tree is created
}
```