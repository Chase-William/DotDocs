using DotDocs.Markdown.Enums;
using DotDocs.Markdown.Extensions;
using DotDocs.Markdown.Renderers.Components;
using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown.Renderers.Members
{
    public class EventRenderer : IMemberRenderer
    {
        public IComponentRenderer<EventInfo> DeclarationRenderer { get; init; }

        public EventRenderer(IComponentRenderer<EventInfo> declarationRenderer)
            => DeclarationRenderer = declarationRenderer;

        public void Render<T>(T info) where T : MemberInfo
        {
            if (!info.Check(out ArgumentException ex, out EventInfo _event))
            {
                IMemberRenderer.Logger.Fatal(ex);
                throw ex;
            }

            AsMarkdown.H3.Prefix(_event.Name, padding: Padding.DoubleNewLine);

            DeclarationRenderer.Render(_event, Padding.DoubleNewLine);

            if (_event.TryGetComments(out CommonComments? comments))
            {
                ArgumentNullException.ThrowIfNull(comments);

                _event.PutSummary(comments);
                _event.PutExample(comments);
                _event.PutRemarks(comments);
            }
        }
    }
}
