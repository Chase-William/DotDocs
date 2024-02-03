namespace DotDocs.Models
{
    public class RepositoryModel
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public string Name { get; set; }
        public string Url { get; set; }
        public string Commit { get; set; }
        public DateTime Added { get; set; }
        public List<ProjectModel> Projects { get; set; } = new();

        public RepositoryModel()
        {
            Logger.Debug("Default Constructor.");
        }
    }
}