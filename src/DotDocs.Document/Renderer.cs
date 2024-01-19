using DotDocs.Models;

namespace DotDocs.Document
{
    public class Renderer
    {
        internal RepositoryModel RepoModel { get; init; }

        public Renderer(RepositoryModel repoModel)
            => RepoModel = repoModel;

        public void Render()
        {
            Console.WriteLine();
            //foreach (var project in RepoModel.Projects)
            //{
            //    project.Visit();
            //}
        }    
    }

    //public static class Extensions
    //{
    //    public static void Visit(this ProjectModel proj)
    //    {
    //        foreach (var project in proj.Projects)
    //        {
    //            project.Visit();
    //        }
    //    }
    //}
}