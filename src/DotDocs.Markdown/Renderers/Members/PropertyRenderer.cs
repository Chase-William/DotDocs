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
    public class PropertyRenderer : IMemberRenderer
    {
        public IComponentRenderer<PropertyInfo> DeclarationRenderer { get; init; }

        /// <summary>
        /// Stores optional rendering styles to be included.
        /// </summary>
        private readonly AsMarkdown style;

        public PropertyRenderer(IComponentRenderer<PropertyInfo> declarationRenderer, bool includeHorizontalRule = false)
        {
            DeclarationRenderer = declarationRenderer;
            style = includeHorizontalRule ? AsMarkdown.HorizonalLine : AsMarkdown.None;
        }

        public void Render<T>(T info) where T : MemberInfo
        {
            if (!info.Check(out ArgumentException ex, out PropertyInfo property))
            {
                IMemberRenderer.Logger.Fatal(ex);
                throw ex;
            }

            AsMarkdown.H3.Prefix(property.Name, padding: Padding.DoubleNewLine);

            // Check if hr should be rendered
            if (style.HasFlag(AsMarkdown.HorizonalLine))
                AsMarkdown.HorizonalLine.Put(Padding.DoubleNewLine);

            DeclarationRenderer.Render(property, Padding.DoubleNewLine);    

            if (property.TryGetComments(out CommonComments? comments))
            {
                ArgumentNullException.ThrowIfNull(comments);

                property.PutSummary(comments);
                property.PutExample(comments);
                property.PutRemarks(comments);
            }
        }
    }
}
