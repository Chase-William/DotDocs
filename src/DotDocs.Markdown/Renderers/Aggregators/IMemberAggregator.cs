using DotDocs.Markdown.Renderers.Members;
using DotDocs.Markdown.Renderers.Sections;
using DotDocs.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown.Renderers.Aggregators
{
    /// <summary>
    /// An interface relating a <see cref="IMemberRenderer"/> to a data set provided from <see cref="MemberAggregationFunc"/>.
    /// </summary>
    public interface IMemberAggregator
    {
        /// <summary>
        /// Provides an interface for rendering results from the <see cref="MemberAggregationFunc"/>.
        /// </summary>
        public IMemberRenderer MemberRenderer { get; }

        /// <summary>
        /// Collects and returns a set <see cref="MemberInfo"/> types.
        /// </summary>
        public Func<Type, IEnumerable<MemberInfo>> MemberAggregationFunc { get; }


        public ISectionRenderer? HeaderRenderer { get; }
        public ISectionRenderer? FooterRenderer { get; }

        /// <summary>
        /// By default each item from the <see cref="MemberAggregationFunc"/>'s results will be rendered using the set <see cref="MemberRenderer"/>.
        /// </summary>
        public void RenderMembers(Type type)
        {
            HeaderRenderer?.Render(type);
            foreach (var member in MemberAggregationFunc(type))            
                MemberRenderer.Render(member);
            FooterRenderer?.Render(type);
        }
    }
}
