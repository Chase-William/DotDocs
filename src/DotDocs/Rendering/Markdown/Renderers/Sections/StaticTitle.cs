using DotDocs.Markdown.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown.Renderers.Sections
{
    /// <summary>
    /// Renders a simple string as a title.
    /// </summary>
    internal class StaticTitle : ISectionRenderer
    {
        private readonly string title;

        private readonly AsMarkdown style;

        public StaticTitle(string title, AsMarkdown style)
        {
            this.title = title;
            this.style = style;
        }

        public void Render(Type type)
        {
            style.Prefix(title, padding: Padding.DoubleNewLine);
        }
    }
}
