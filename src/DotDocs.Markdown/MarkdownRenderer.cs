using DotDocs.IO;
using DotDocs.Models;
using DotDocs.Models.Language;
using DotDocs.Render;
using DotDocs.Render.Args;
using System.Collections.Immutable;

namespace DotDocs.Markdown
{
    public class MarkdownRenderer : IRenderable
    {
        public RepositoryModel Model { get; set; }

        public ImmutableDictionary<string, ProjectModel> Projects { get; set; }

        public IOutputable Output { get; set; }
         

        public void RenderType(TypeModel model)
        {
            try
            {
                File.Create(Path.Combine(Output.GetValue(), model.Name + ".md"));
            }
            catch
            {
                throw;
            }
        }
    }

    //public static class Extensions
    //{
    //    public static void Render(this TypeModel model, IOutputable output)
    //    {
    //        string fullPath = model.FullName.Replace('.', '/');
    //        try
    //        {
    //            // File.Create(Path.Combine(output. fullPath + ".md");
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //    }
    //}
}