using DotDocs.IO;
using DotDocs.Models;
using System.Collections.Immutable;
using System.Text;

namespace DotDocs.Render
{
    public delegate void RenderType(Type type);

    public interface IRenderer
    {
        public RepositoryModel Model { get; set; }
        public ImmutableDictionary<string, ProjectModel> Projects { get; set; }
        public IOutputable Output { get; set; }

        public void Init(
            RepositoryModel model,
            ImmutableDictionary<string, ProjectModel> projects);

        public event RenderType RenderClass;
        public event RenderType RenderStruct;
        public event RenderType RenderInterface;
        public event RenderType RenderDelegate;
        public event RenderType RenderEnum;

        public void Render();            
    }
}
