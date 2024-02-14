using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown.Renderers.Sections
{
    /// <summary>
    /// An interface for rendering sections like headers and footers.
    /// </summary>
    public interface ISectionRenderer
    {
        /// <summary>
        /// Render the section.
        /// </summary>
        /// <param name="type">The declaring type.</param>
        public void Render(Type type);
    }
}
