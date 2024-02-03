using DotDocs.Markdown.Renderers.Aggregators;
using DotDocs.Markdown.Renderers.Sections;

namespace DotDocs.Markdown.Renderers.Types
{
    /// <summary>
    /// An implementation for rendering a <c>enum</c>.
    /// </summary>
    public class EnumRenderer : ITypeRenderer
    {
        public IMemberAggregator[] MemberAggregators { get; init; }

        public ISectionRenderer? Header { get; init; }

        public ISectionRenderer? Footer { get; init; }

        public EnumRenderer(ISectionRenderer header, params IMemberAggregator[] memberAggregators) : this(header, null, memberAggregators) { }

        public EnumRenderer(
            ISectionRenderer? header,
            ISectionRenderer? footer,
            params IMemberAggregator[] memberAggregators
            ) {
            this.MemberAggregators = memberAggregators;
            Header = header;
            Footer = footer;
        }
    }
}
