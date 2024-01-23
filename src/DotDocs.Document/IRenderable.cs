using DotDocs.IO;
using DotDocs.Models;
using System.Collections.Immutable;

namespace DotDocs.Render
{
    public interface IRenderable
    {
        public RepositoryModel Model { get; set; }
        public ImmutableDictionary<string, ProjectModel> Projects { get; set; }
        public IOutputable Output { get; set; }

        public void Prepare(
            RepositoryModel model,
            ImmutableDictionary<string, ProjectModel> projects,
            IOutputable output)
        {
            Model = model;
            Projects = projects;
            Output = output;
        }

        public void Render()
        {
            foreach (var proj in Projects.Values)
                foreach (var type in proj.Assembly.ExportedTypes)
                    RenderType(type);
        }

        public void RenderType(Type type);
    }
}
