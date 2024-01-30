using DotDocs.IO;
using DotDocs.Models;
using System.Collections.Immutable;
using System.Text;

namespace DotDocs.Render
{
    public interface IRenderer
    {
        public RepositoryModel Model { get; set; }
        public ImmutableDictionary<string, ProjectModel> Projects { get; set; }
        public IOutputable Output { get; set; }

        public void Init(
            RepositoryModel model,
            ImmutableDictionary<string, ProjectModel> projects);

        public void Render();    

        public void RenderClass(Type type, StringBuilder builder);

        public void RenderStruct(Type type, StringBuilder builder);

        public void RenderInterface(Type type, StringBuilder builder);

        public void RenderDelegate(Type type, StringBuilder builder);

        public void RenderEnum(Type type, StringBuilder builder);
    }
}
