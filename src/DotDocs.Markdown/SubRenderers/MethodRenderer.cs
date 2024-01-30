using DocXml.Reflection;
using DotDocs.Markdown;
using DotDocs.Markdown.Components;
using DotDocs.Markdown.Enums;
using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using State = DotDocs.Markdown.RenderState;

namespace DotDocs.Markdown.SubRenderers
{    
    public class MethodRenderer
    {
        public IComponentRenderer<MethodInfo> DeclarationRenderer { get; init; }

        public MethodRenderer(IComponentRenderer<MethodInfo> declarationRenderer)
        {
            DeclarationRenderer = declarationRenderer;
        }

        public void Render(MethodInfo info)
        {
            // Method Header
            AsMarkdown.H3.Prefix(info.Name, padding: Padding.DoubleNewLine);
            

            // Smaller method declaration
            // $"{info.ReturnType.AsMaybeLink()} {info.Name}".Put();
            // Create parameter listing
            // info.GetParameters().PutParameters();            

            DeclarationRenderer.Render(info, Padding.DoubleNewLine);

            //AsStyled.CodeBlock.OpenCodeBlock();
            
            //info.ReturnType.ToNameString().Put();
            //" ".Put();
            //info.Name.Put();
            //" ".Put();
            //info.GetParameters().PutParameters(LinePadding.NewLine);

            //AsStyled.CodeBlock.CloseCodeBlock();

            MethodComments? comments = info.GetComments<MethodComments>();

            // Put the list of type args, parameters, and return all with comments
            var argsPut = info.PutTypeArgsWithComments(comments);            
            var paramsPut = info.PutParametersWithComments(comments);
            var returnPut = info.PutReturnWithComments(comments);

            // Put a newline if any of the args, params, or return rendered          
            if (argsPut || paramsPut || returnPut)
                Padding.NewLine.Put();

            // Only render summary, example, and remarks if the comment is not null
            if (comments is not null)
            {
                info.PutSummary(comments);
                info.PutExample(comments);
                info.PutRemarks(comments);
            }
        }                
    }
}
