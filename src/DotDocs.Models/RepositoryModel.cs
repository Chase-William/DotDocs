using Neo4j.Driver;
using GDC = DotDocs.Models.GraphDatabaseConnection;

namespace DotDocs.Models
{
    public class RepositoryModel : Model
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Commit { get; set; }
        public DateTime Added { get; set; }
        public List<ProjectModel> Projects { get; set; } = new();

        public void Run()
        {
            BulkInsertEntireRepository();
            Console.WriteLine();
        }

        internal void BulkInsertEntireRepository()
        {
            var repoResult = Insert();
            Projects.First().BulkInsertEntireProject();


            //return await session.ExecuteWriteAsync(async tx => {
            //    var result = await tx.RunAsync(query, new { name = Name });
            //    return await result.SingleAsync();
            //});           
        }

        public IResultSummary Insert()
        {
            var session = GDC.driver.Session(o => o.WithDatabase("neo4j"));

            string query = @"
                CREATE (r:Repository { 
                    name: $name,
                    url: $url,
                    commit: $commit
                })";

            try
            {
                IResult cursor = session.Run(query, new
                {
                    name = Name,
                    url = Url,
                    commit = Commit
                });
                return cursor.Consume();
            }
            finally
            {
                session.Dispose();
            }
        }
    }
}