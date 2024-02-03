using DocXml.Reflection;
using DotDocs.Markdown;
using DotDocs.Markdown.Enums;
using DotDocs.Markdown.Renderers.Components;
using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using State = DotDocs.Markdown.RenderState;

namespace DotDocs.Markdown.Renderers.Members
{    
    public class MethodRenderer : IMemberRenderer
    {
        public IComponentRenderer<MethodInfo> DeclarationRenderer { get; init; }

        public MethodRenderer(IComponentRenderer<MethodInfo> declarationRenderer)
        {
            DeclarationRenderer = declarationRenderer;
        }

        public void Render<T>(T info) where T : MemberInfo
        {
            if (!info.Check(out ArgumentException ex, out MethodInfo method))
            {
                IMemberRenderer.Logger.Fatal(ex);
                throw ex;
            }

            // Method Header
            AsMarkdown.H3.Prefix(method.Name, padding: Padding.DoubleNewLine);
                        
            DeclarationRenderer.Render(method, Padding.DoubleNewLine);

            MethodComments? comments = info.GetComments<MethodComments>();

            // Put the list of type args, parameters, and return all with comments
            var argsPut = method.PutTypeArgsWithComments(comments);            
            var paramsPut = method.PutParametersWithComments(comments);
            var returnPut = method.PutReturnWithComments(comments);

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
