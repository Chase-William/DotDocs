## Issues

### Poor location mechanism for finding package deps

Use the .csproj to locate project.assets.json and then resolve all nuget dependencies.

- Nuget dependencies can be located in multiple places so best to check until exhausted or located.
- Should build this into it's own library
- Provide testing for this library and release it as it's own mini package
- How to vendor this to Charp then for easy automation?