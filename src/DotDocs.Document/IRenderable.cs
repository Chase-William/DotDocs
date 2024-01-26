using DotDocs.IO;
using DotDocs.Models;
using System.Collections.Immutable;
using System.Text;

namespace DotDocs.Render
{
    public interface IRenderable
    {
        public RepositoryModel Model { get; set; }
        public ImmutableDictionary<string, ProjectModel> Projects { get; set; }
        public IOutputable Output { get; set; }

        public void Init(
            RepositoryModel model,
            ImmutableDictionary<string, ProjectModel> projects);

        public void Render();    

        public void RenderType(Type type, StringBuilder builder);

        public void RenderEnum(Type type, StringBuilder builder);
    }
}
