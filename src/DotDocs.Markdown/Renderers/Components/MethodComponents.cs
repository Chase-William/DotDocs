using DocXml.Reflection;
using DotDocs.Markdown.Enums;
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
    /// Format the method declaration into output normally.
    /// </summary>
    public class MethodDeclaration : IComponentRenderer<MethodInfo>
    {
        public void Render(MethodInfo info, Padding padding = Padding.DoubleNewLine)
        {
            AsMarkdown.HorizonalLine.Put(Padding.DoubleNewLine);

            info.ReturnType.PutTypeName(info.DeclaringType!, Padding.Space);
            info.Name.Put();

            // Put generic type arguments if they exists
            info.PutTypeArgs();            

            AsGeneral.OpeningParenthese.Put();
            info.GetParameters().ToMarkdown(each: (parameter, index) =>
            {
                if (index != 0)
                    AsGeneral.Comma.Put(Padding.Space);
                parameter.ParameterType.PutTypeName(info.DeclaringType!, Padding.Space);
                AsMarkdown.Italic.Wrap(parameter.Name!);
            });
            AsGeneral.ClosingParenthese.Put();

            padding.Put();
        }
    }

    /// <summary>
    /// Format the method declaration into output encased in a codeblock.
    /// </summary>
    public class MethodCodeBlockDeclaration : IComponentRenderer<MethodInfo>
    {
        public void Render(MethodInfo info, Padding padding = Padding.DoubleNewLine)
        {
            AsMarkdown.CodeBlock.OpenCodeBlock();

            info.ReturnType.ToNameString().Put();
            Padding.Space.Put();
            info.Name.Put();


            // Put generic type arguments if they exists
            info.PutTypeArgs();
            
            AsGeneral.OpeningParenthese.Put();
            info.GetParameters().ToMarkdown(each: (parameter, index) =>
            {
                if (index != 0)
                    AsGeneral.Comma.Put(Padding.Space);
                parameter.ParameterType.Name.Put(Padding.Space);
                parameter.Name!.Put();
            });
            AsGeneral.ClosingParenthese.Put();

            Padding.NewLine.Put();
            AsMarkdown.CodeBlock.CloseCodeBlock(padding);
        }
    }

    public static class MethodEx
    {
        const string PUBLIC = "public";
        const string INTERNAL = "internal";
        const string PROTECTED = "protected";
        const string PRIVATE = "private";
        const string INTERNAL_PROTECTED = "internal protected";

        /// <summary>
        /// Gets the modifiers source code name from the given <see cref="MethodAttributes"/>. Only works with accessibility modifiers.
        /// </summary>
        /// <param name="attr">Accessibility modifier attributes.</param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        public static string GetAccessibilityString(this MethodAttributes attr)
        {
            if (attr.HasFlag(MethodAttributes.Public))
                return PUBLIC;
            else if (attr.HasFlag(MethodAttributes.Assembly))
                return INTERNAL;
            else if (attr.HasFlag(MethodAttributes.Family))
                return PROTECTED;
            else if (attr.HasFlag(MethodAttributes.Private))
                return PRIVATE;
            else if (attr.HasFlag(MethodAttributes.FamORAssem))
                return INTERNAL_PROTECTED;
            else
                throw new InvalidEnumArgumentException();
        }
    }
}
