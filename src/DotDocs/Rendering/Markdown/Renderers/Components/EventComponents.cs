using DocXml.Reflection;
using DotDocs.Markdown.Enums;
using DotDocs.Markdown.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown.Renderers.Components
{
    public class EventDeclaration: IComponentRenderer<EventInfo>
    {
        public void Render(EventInfo info, Padding padding)
        {
            info.EventHandlerType!.PutTypeName(info.DeclaringType!, Padding.Space);
            info.Name.Put(padding);
        }
    }

    public class EventCodeBlockDeclaration : IComponentRenderer<EventInfo>
    {
        public void Render(EventInfo info, Padding padding)
        {
            AsMarkdown.CodeBlock.OpenCodeBlock();

            info.EventHandlerType!.ToNameString().Put(Padding.Space);
            info.Name.Put(Padding.NewLine);

            AsMarkdown.CodeBlock.CloseCodeBlock();
        }
    }
}
