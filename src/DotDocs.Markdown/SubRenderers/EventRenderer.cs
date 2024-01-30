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
    public class EventRenderer
    {
        public IComponentRenderer<EventInfo> DeclarationRenderer { get; init; }

        public EventRenderer(IComponentRenderer<EventInfo> declarationRenderer)
            => DeclarationRenderer = declarationRenderer;

        public void Render(EventInfo info)
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
