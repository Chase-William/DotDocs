using DotDocs.Markdown.Renderers.Aggregators;
using DotDocs.Markdown.Renderers.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown.Renderers.Types
{
    /// <summary>
    /// An implementation for rendering an <c>interface</c>.
    /// </summary>
    public class InterfaceRenderer : ITypeRenderer
    {
        public IMemberAggregator[] MemberAggregators { get; init; }

        public ISectionRenderer? Header { get; init; }

        public ISectionRenderer? Footer { get; init; }

        public InterfaceRenderer(ISectionRenderer header, params IMemberAggregator[] memberAggregators) : this(header, null, memberAggregators) { }

        public InterfaceRenderer(
            ISectionRenderer? header = null,
            ISectionRenderer? footer = null,
            params IMemberAggregator[] memberAggregators)
        {
            this.MemberAggregators = memberAggregators;
            Header = header;
            Footer = footer;
        }
    }
}
