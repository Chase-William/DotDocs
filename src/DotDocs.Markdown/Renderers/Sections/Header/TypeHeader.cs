using DotDocs.Markdown.Enums;
using DotDocs.Markdown.Extensions;
using LoxSmoke.DocXml;
using NLog;

namespace DotDocs.Markdown.Renderers.Sections.Header
{
    public class TypeHeader : ISectionRenderer
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public void Render(Type type)
        {
            Logger.Trace("Rendering header for {type}", type.FullName);

            AsMarkdown.H1.Put(Padding.Space);
            type.PutTypeName(null, Padding.DoubleNewLine);

            // Put comments for the given type
            if (type.TryGetComments(out TypeComments? comments))
            {
                ArgumentNullException.ThrowIfNull(comments);

                type.PutSummary(comments);
                type.PutExample(comments);
                type.PutRemarks(comments);
            }
        }
    }
}
