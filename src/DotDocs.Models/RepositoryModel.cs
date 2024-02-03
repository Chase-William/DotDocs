namespace DotDocs.Models
{
    public class RepositoryModel
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public List<ProjectModel> Projects { get; set; } = new();

        public RepositoryModel()
        {
            Logger.Debug("Default Constructor.");
        }
    }
}