using DotDocs.IO;
using DotDocs.Models;
using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Text;

namespace DotDocs.Rendering
{
    public delegate void RenderType(Type type);

    public interface IRenderer
    {
        public ImmutableArray<AssemblyReflectInfo> Assemblies { get; set; }
        public IOutputable Output { get; set; }

        public void Init(ImmutableArray<AssemblyReflectInfo> assemblies);

        public event RenderType RenderClass;
        public event RenderType RenderStruct;
        public event RenderType RenderInterface;
        public event RenderType RenderDelegate;
        public event RenderType RenderEnum;

        public void Render();
    }
}
