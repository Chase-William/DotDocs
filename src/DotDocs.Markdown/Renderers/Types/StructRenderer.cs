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
    /// An implemenation for rendering a <c>struct</c>.
    /// </summary>
    public class StructRenderer : ITypeRenderer
    {
        public IMemberAggregator[] MemberAggregators { get; init; }

        public ISectionRenderer? Header { get; init; }

        public ISectionRenderer? Footer { get; init; }

        public StructRenderer(ISectionRenderer header, params IMemberAggregator[] memberAggregators) : this(header, null, memberAggregators) { }

        public StructRenderer(
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
