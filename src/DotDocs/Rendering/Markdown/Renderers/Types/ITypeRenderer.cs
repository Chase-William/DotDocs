using DotDocs.Markdown.Renderers.Aggregators;
using DotDocs.Markdown.Renderers.Sections;
using System;

namespace DotDocs.Markdown.Renderers.Types
{
    /// <summary>
    /// An interface relating a <see cref="Type"/> and the means for collecting and rendering its members.
    /// </summary>
    public interface ITypeRenderer
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// A collection of <see cref="IMemberAggregator"/>s used for collecting members and rendering them.
        /// </summary>
        public IMemberAggregator[] MemberAggregators { get; }
        /// <summary>
        /// A section to be rendered as a header.
        /// </summary>
        public ISectionRenderer? Header { get; }
        /// <summary>
        /// A section to be rendered as a footer.
        /// </summary>
        public ISectionRenderer? Footer { get; }
        /// <summary>
        /// Renders the given type's group of members sequentially.
        /// </summary>
        /// <param name="type">Declaring type of members to be rendered.</param>
        public void Render(Type type)
        {            
            Header?.Render(type);
            Logger.Trace("Rendering members for {type}", type.FullName);
            foreach (var aggregator in MemberAggregators)            
                aggregator.RenderMembers(type);
            Footer?.Render(type);
        }
    }
}
