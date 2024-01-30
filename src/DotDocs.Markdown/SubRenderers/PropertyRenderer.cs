using DotDocs.Markdown.Components;
using DotDocs.Markdown.Enums;
using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown.SubRenderers
{
    public class PropertyRenderer
    {
        public IComponentRenderer<PropertyInfo> DeclarationRenderer { get; init; }

        public PropertyRenderer(IComponentRenderer<PropertyInfo> declarationRenderer)
            => DeclarationRenderer = declarationRenderer;

        public void Render(PropertyInfo info)
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
