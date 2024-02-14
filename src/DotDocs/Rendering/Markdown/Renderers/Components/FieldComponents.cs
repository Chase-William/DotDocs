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
    public class FieldDeclaration : IComponentRenderer<FieldInfo>
    {
        public void Render(FieldInfo info, Padding padding)
        {
            info.FieldType.PutTypeName(info.DeclaringType!, Padding.Space);
            info.Name.Put(padding);
        }
    }

    public class FieldCodeBlockDeclaration : IComponentRenderer<FieldInfo>
    {
        public void Render(FieldInfo info, Padding padding)
        {
            AsMarkdown.CodeBlock.OpenCodeBlock();
            info.FieldType.ToNameString().Put(Padding.Space);
            info.Name.Put(Padding.NewLine);
            AsMarkdown.CodeBlock.CloseCodeBlock();
        }
    }
}
