using DotDocs.Markdown.Enums;
using DotDocs.Markdown.Extensions;
using DotDocs.Markdown.Renderers.Components;
using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown.Renderers.Members
{
    public class EnumValueRenderer : IMemberRenderer
    {
        public void Render<T>(T info) where T : MemberInfo
        {
            IMemberRenderer.Logger.Trace("Rendering for member {member}", info.Name);

            if (!info.Check(out ArgumentException ex, out FieldInfo field))
            {
                IMemberRenderer.Logger.Fatal(ex);
                throw ex;
            }

            AsMarkdown.UnorderedListItem.Prefix(field.Name, AsMarkdown.Code);

            if (field.TryGetComments(out CommonComments? comments))
            {
                ArgumentNullException.ThrowIfNull(comments);

                if (!string.IsNullOrWhiteSpace(comments.Summary))                
                    AsGeneral.Comma.Prefix(comments.Summary, true);                
            }
            Padding.NewLine.Put();
        }
    }
}
