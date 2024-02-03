using DotDocs.Markdown.Renderers.Members;
using DotDocs.Markdown.Renderers.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown.Renderers.Aggregators
{
    /// <summary>
    /// A default implementation for the <see cref="IMemberRenderer"/> interface.
    /// </summary>
    internal class MemberAggregator : IMemberAggregator
    {
        public Func<Type, IEnumerable<MemberInfo>> MemberAggregationFunc { get; init; }

        public IMemberRenderer MemberRenderer { get; set; }

        public ISectionRenderer? HeaderRenderer { get; init; }

        public ISectionRenderer? FooterRenderer { get; init; }

        public MemberAggregator(
            IMemberRenderer memberRenderer,
            Func<Type, IEnumerable<MemberInfo>> memberAggregationFunc,
            ISectionRenderer? headerRenderer = null,
            ISectionRenderer? footerRenderer = null
            ) {
            this.MemberRenderer = memberRenderer;
            this.MemberAggregationFunc = memberAggregationFunc;
            HeaderRenderer = headerRenderer;
            FooterRenderer = footerRenderer;
        }
    }
}
