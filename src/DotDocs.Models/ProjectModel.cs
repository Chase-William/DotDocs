using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDC = DotDocs.Models.GraphDatabaseConnection;

namespace DotDocs.Models
{
    public class ProjectModel : Model
    {
        public string Name { get; set; }
        public string SDK { get; set; }
        public string TargetFramework { get; set; }
        public AssemblyModel Assembly { get; set; }
        public List<ProjectModel> Projects { get; set; } = new();

        internal void BulkInsertEntireProject()
        {            
            // Insert in a DFS order
            foreach (var proj in Projects)            
                proj.BulkInsertEntireProject();
            Insert();
        }

        public void Insert()
        {
            var session = GDC.driver.Session(o => o.WithDatabase("neo4j"));

            string query = @"
                CREATE (r:Project { 
                    name: $name
                })";            

            try
            {
                // Insert project
                IResult cursor = session.Run(query, new
                {
                    name = Name
                });

                // var test = cursor.Consume();

                

                // var currentProjectId = records.First().As<ProjectModel>().Id;

                // Insert Relationships for projects if needed
                //if (Projects.Count > 0)
                //{
                    
                //    foreach (var proj in Projects)
                //    {
                //        session.Run(@"
                //            MATCH (this:Project { id: $tid }), (other:Project { id: $oid }) 
                //            CREATE (this)-[r:USES]->(other)",
                //            new
                //            {
                //                tid = currentProjectId,
                //                oid = proj.Id
                //            });
                //    }                    
                //}                
            }
            finally
            {
                session.Dispose();
            }
        }
    }
}
