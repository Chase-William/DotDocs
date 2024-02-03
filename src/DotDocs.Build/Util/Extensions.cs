using DotDocs.Build.Build;
using DotDocs.Models;

namespace DotDocs.Build.Util
{
    /// <summary>
    /// A static class that exists purely to contain extension methods.
    /// </summary>
    public static class Extensions
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static RepositoryModel Apply(
            this RepositoryModel model, 
            Repository repo,
            Dictionary<string, ProjectModel> projects
            ) {
            Logger.Trace("Applying updates to {repoModelType} from {repo}", typeof(RepositoryModel).FullName, typeof(Repository).FullName);

            // Handles creation of the project and all down stream entities
            var rootProject = repo.build.GetRootProject(projects);
            model.Projects.Add(rootProject);
            return model;
        }        
    }
}
