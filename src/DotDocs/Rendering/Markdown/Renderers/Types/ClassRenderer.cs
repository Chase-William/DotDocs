using DotDocs.Markdown.Renderers.Aggregators;
using DotDocs.Markdown.Renderers.Members;
using DotDocs.Markdown.Renderers.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown.Renderers.Types
{
    /// <summary>
    /// An implementation for rendering a <c>class</c>.
    /// </summary>
    public class ClassRenderer : ITypeRenderer
    {
        public IMemberAggregator[] MemberAggregators { get; init; }

        public ISectionRenderer? Header { get; init; }

        public ISectionRenderer? Footer { get; init; }

        public ClassRenderer(ISectionRenderer header, params IMemberAggregator[] memberAggregators) : this(header, null, memberAggregators) { }

        public ClassRenderer(
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
