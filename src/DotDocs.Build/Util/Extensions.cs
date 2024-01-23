using DotDocs.Build.Build;
using DotDocs.Models;

namespace DotDocs.Build.Util
{
    /// <summary>
    /// A static class that exists purely to contain extension methods.
    /// </summary>
    public static class Extensions
    {        
        public static RepositoryModel Apply(
            this RepositoryModel model, 
            Repository repo,
            Dictionary<string, ProjectModel> projects
            ) {
            model.Name = repo.Name;
            model.Url = repo.Url;
            model.Commit = repo.Commit;
            // Handles creation of the project and all down stream entities
            var rootProject = repo.build.GetRootProject(projects);
            model.Projects.Add(rootProject);
            return model;
        }        
    }
}
