using DotDocs.Markdown.Enums;
using DotDocs.Markdown.Renderers.Components;
using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using State = DotDocs.Markdown.RenderState;

namespace DotDocs.Markdown.Renderers.Members
{
    public class FieldRenderer : IMemberRenderer
    {
        public IComponentRenderer<FieldInfo> DeclarationRenderer { get; init; }

        public FieldRenderer(IComponentRenderer<FieldInfo> declarationRenderer)
            => DeclarationRenderer = declarationRenderer;

        public void Render<T>(T info) where T : MemberInfo
        {
            if (!info.Check(out ArgumentException ex, out FieldInfo field))
            {
                IMemberRenderer.Logger.Fatal(ex);
                throw ex;
            }

            AsMarkdown.H3.Prefix(field.Name, padding: Padding.DoubleNewLine);

            DeclarationRenderer.Render(field, Padding.None);

            if (field.TryGetComments(out CommonComments? comments))
            {
                ArgumentNullException.ThrowIfNull(comments);

                field.PutSummary(comments);
                field.PutExample(comments);
                field.PutRemarks(comments);
            }
        }
    }
}
