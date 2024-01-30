using DotDocs.Markdown.Components;
using DotDocs.Markdown.Enums;
using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using State = DotDocs.Markdown.RenderState;

namespace DotDocs.Markdown.SubRenderers
{
    public class FieldRenderer
    {
        public IComponentRenderer<FieldInfo> DeclarationRenderer { get; init; }

        public FieldRenderer(IComponentRenderer<FieldInfo> declarationRenderer)
            => DeclarationRenderer = declarationRenderer;

        public void Render(FieldInfo info)
        {
            AsMarkdown.H3.Prefix(info.Name, padding: Padding.DoubleNewLine);

            DeclarationRenderer.Render(info, Padding.None);

            if (info.TryGetComments(out CommonComments? comments))
            {
                ArgumentNullException.ThrowIfNull(comments);

                info.PutSummary(comments);
                info.PutExample(comments);
                info.PutRemarks(comments);
            }
        }
    }
}
