using Neo4j.Driver;
using System.Text.Json.Serialization;
using GDC = DotDocs.Models.GraphDatabaseConnection;
using Newtonsoft.Json;
using DotDocs.Models.Util;

namespace DotDocs.Models
{
    public class ProjectModel : Model
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("sdk")]
        public string SDK { get; set; } = string.Empty;
        [JsonProperty("targetFramework")]
        public string TargetFramework { get; set; } = string.Empty;
        [Newtonsoft.Json.JsonIgnore]
        public AssemblyModel Assembly { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public List<ProjectModel> Projects { get; set; } = new();       

        internal void InsertTypes(List<AssemblyModel> assemblies)
        {
            foreach (var proj in Projects)
                proj.InsertTypes(assemblies);

            Assembly.InsertTypes(assemblies);
        }

        internal void ConnectToAssembly()
        {
            foreach (var proj in Projects)
                proj.ConnectToAssembly();

            var query = GDC.Client.Cypher
                .Match("(p:Project { uid: $suid }), (a:Assembly { uid: $ruid })")
                .WithParams(new
                {
                    suid = UID,
                    ruid = Assembly.UID
                })
                .Create("(p)-[rel:PRODUCES]->(a)");

            try
            {
                query.ExecuteWithoutResultsAsync().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
            //finally
            //{
            //    await session.CloseAsync();
            //}
        }

        internal void GetProducedAssemblies(List<AssemblyModel> assemblies)
        {
            foreach (var proj in Projects)
                proj.GetProducedAssemblies(assemblies);

            if (!assemblies.Contains(Assembly))
                assemblies.Add(Assembly);
        }

        internal void Flat(List<ProjectModel> projects)
        {
            foreach (var proj in Projects)
                proj.Flat(projects);
            projects.Add(this);
        }

        internal void ConnectProjects()
        {
            foreach (var item in Projects)            
                item.ConnectProjects();

            // Prevent query from operating on an empty list
            if (Projects.Count == 0)
                return;

            foreach (var proj in Projects)
            {
                // using var session = GDC.GetSession();

                try
                {
                    // suid == sender id, ruid == receiving id
                    var r = GDC.Client.Cypher
                        .Match("(p1:Project { uid: $suid }), (p2:Project { uid: $ruid })")
                        .WithParams(new
                        {
                            suid = UID,
                            ruid = proj.UID
                        })
                        .Create("(p1)-[r:USES]->(p2)");

                    r.ExecuteWithoutResultsAsync().Wait();                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                }
            }                       
        }
    }
}
