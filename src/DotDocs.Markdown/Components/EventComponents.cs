using DocXml.Reflection;
using DotDocs.Markdown.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown.Components
{
    public class EventDeclaration: IComponentRenderer<EventInfo>
    {
        public void Render(EventInfo info, Padding padding)
        {
            info.EventHandlerType!.MaybeLink(info.DeclaringType!, Padding.Space);
            info.Name.Put(Padding.NewLine);
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
