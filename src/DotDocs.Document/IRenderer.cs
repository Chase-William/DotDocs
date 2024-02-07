using DotDocs.IO;
using DotDocs.Models;
using System.Collections.Immutable;
using System.Reflection;
using System.Text;

namespace DotDocs.Render
{
    public delegate void RenderType(Type type);

    public interface IRenderer
    {
        public ImmutableArray<(string docs, Assembly binary)> Assemblies { get; set; }
        public IOutputable Output { get; set; }

        public void Init(ImmutableArray<(string docs, Assembly binary)> assemblies);

        public event RenderType RenderClass;
        public event RenderType RenderStruct;
        public event RenderType RenderInterface;
        public event RenderType RenderDelegate;
        public event RenderType RenderEnum;

        public void Render();            
    }
}
