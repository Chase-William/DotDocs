using DocXml.Reflection;
using DotDocs.Markdown.Enums;
using DotDocs.Markdown.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown.Renderers.Components
{
    /// <summary>
    /// Displays the property like C# source code.
    /// </summary>
    public class PropertyDeclaration : IComponentRenderer<PropertyInfo>
    {
        public void Render(PropertyInfo info, Padding padding)
        {
            info.PropertyType.PutTypeName(info.DeclaringType!, Padding.Space);
            info.Name.Put(Padding.Space);

            AsGeneral.OpeningCurly.Put(Padding.Space);
            // Render Get
            if (info.CanRead)
            {
                if (!info.GetMethod!.Attributes.HasFlag(MethodAttributes.Public))
                    AsMarkdown.BoldItalic.Wrap(info.GetMethod!.Attributes.GetAccessibilityString(), Padding.Space);
                AsMarkdown.Italic.Wrap("get");
                AsGeneral.SemiColon.Put(Padding.Space);
            }
            // Render Set
            if (info.CanWrite)
            {
                if (!info.SetMethod!.Attributes.HasFlag(MethodAttributes.Public))
                    AsMarkdown.BoldItalic.Wrap(info.SetMethod!.Attributes.GetAccessibilityString(), Padding.Space);
                AsMarkdown.Italic.Wrap("set");
                AsGeneral.SemiColon.Put(Padding.Space);
            }
            AsGeneral.ClosingCurly.Put(padding);
        }
    }

    /// <summary>
    /// Displays the property like C# source code inside a C# code block.
    /// </summary>
    public class PropertyCodeBlockDeclaration : IComponentRenderer<PropertyInfo>
    {
        public void Render(PropertyInfo info, Padding padding)
        {
            AsMarkdown.CodeBlock.OpenCodeBlock();
            info.PropertyType.ToNameString().Put(Padding.Space);
            info.Name.Put(Padding.Space);
            // Put get/set info here            
            AsGeneral.OpeningCurly.Put(Padding.Space);
            // Execute if setter or getter is unset or has value but is private
            // Skip checking getter if setter was unset or false as this is only called on public properties and therefore one of the two must be public for this method to run
            if (info.CanRead)
            {
                if (!info.GetMethod!.Attributes.HasFlag(MethodAttributes.Public))
                    info.GetMethod!.Attributes.GetAccessibilityString().Put(Padding.Space);
                "get;".Put(Padding.Space);
            }
            if (info.CanWrite)
            {
                // Write the accessiblity modifier to output if not public
                if (!info.SetMethod!.Attributes.HasFlag(MethodAttributes.Public))
                    info.SetMethod!.Attributes.GetAccessibilityString().Put(Padding.Space);
                "set;".Put(Padding.Space);
            }                                 
            AsGeneral.ClosingCurly.Put(Padding.NewLine);
            AsMarkdown.CodeBlock.CloseCodeBlock();
        }
    }    
}
