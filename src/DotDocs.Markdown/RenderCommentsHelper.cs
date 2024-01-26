using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using State = DotDocs.Markdown.RenderState;

namespace DotDocs.Markdown
{
    public static class RenderCommentsHelper
    {
        /// <summary>
        /// Puts comments for <see cref="FieldInfo"/>, <see cref="PropertyInfo"/>, <see cref="EventInfo"/> into output if they exists. If comments do exists, after their insertion, the standard spacing is appended after the text.
        /// </summary>
        /// <param name="info">To be commented.</param>
        public static void PutComments(this MemberInfo info, bool padding = true)
        {
#if DEBUG
            if (info is MethodInfo)
                throw new Exception($"A less generic overload for handling comments of type {nameof(MethodInfo)} should be used instead.");
#endif

            if (info.TryGetComments<CommonComments>(out var comments))
            {
                State.Builder.Append(comments.Summary);   
                if (padding)
                    Markdown.DOUBLE_NEW_LINE.Put();
            }
        }

        /// <summary>
        /// Puts comments for <see cref="MethodInfo"/> into the output if they exists. If comments do exists, after their insertion, the standard spacing is appended after the text. 
        /// </summary>
        /// <param name="info">To be commented.</param>
        public static void PutComments(this MethodInfo info)
        {
            if (info.TryGetComments<MethodComments>(out var comments))
            {
                State.Builder.Append(comments.Summary);
                State.Builder.Append(comments.Returns);
                Markdown.DOUBLE_NEW_LINE.Put();
            }
        }

        public static void PutComments(this Type info)
        {
            if (info.TryGetComments(out var comments))
            {
                State.Builder.Append(comments.Summary);
                Markdown.DOUBLE_NEW_LINE.Put();
            }
        }
    }
}
