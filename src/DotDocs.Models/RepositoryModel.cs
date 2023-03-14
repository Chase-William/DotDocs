namespace DotDocs.Models
{
    public class RepositoryModel : Model
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Commit { get; set; }
        public DateTime Added { get; set; }
        public List<ProjectModel> Projects { get; set; } = new List<ProjectModel>();
    }
}