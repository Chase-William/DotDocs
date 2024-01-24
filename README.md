![DotDocs_logo_150x150](https://github.com/Chase-William/DotDocs/assets/46757278/ca2b9be4-b1cf-4775-bb04-7a6e5ff7db82)

# DotDocs

Generate easy add hoc docs from your C# codebase in markdown format.

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
