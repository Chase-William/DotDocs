using DotDocs.Markdown;
using DotDocs.Markdown.Enums;
using LoxSmoke.DocXml;
using Microsoft.Management.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using State = DotDocs.Markdown.RenderState;

namespace DotDocs.Markdown
{
    public static class CommentExtensions
    {
        /// <summary>
        /// Renders the summary block to the output stream if it is valid.
        /// </summary>
        /// <param name="_"></param>
        /// <param name="comments"></param>
        public static void PutSummary(this MemberInfo _, CommonComments comments)
        {
            if (!string.IsNullOrWhiteSpace(comments.Summary))
            {
                AsMarkdown.H4.Prefix("Summary", padding: Padding.DoubleNewLine);
                comments.Summary.Put();
                Padding.DoubleNewLine.Put();
            }
        }

        /// <summary>
        /// Renders the example block to the output stream if it is valid.
        /// </summary>
        /// <param name="_"></param>
        /// <param name="comments"></param>
        public static void PutExample(this MemberInfo _, CommonComments comments)
        {
            if (!string.IsNullOrWhiteSpace(comments.Example))
            {
                AsMarkdown.H4.Prefix("Example", padding: Padding.DoubleNewLine);
                comments.Example.Put();
                Padding.DoubleNewLine.Put();
            }
        }

        /// <summary>
        /// Renders the remarks block to the output stream if it is valid.
        /// </summary>
        /// <param name="_"></param>
        /// <param name="comments"></param>
        public static void PutRemarks(this MemberInfo _, CommonComments comments)
        {
            if (!string.IsNullOrWhiteSpace(comments.Remarks))
            {
                AsMarkdown.H4.Prefix("Remarks", padding: Padding.DoubleNewLine);
                comments.Remarks.Put();
                Padding.DoubleNewLine.Put();
            }
        }

        /// <summary>
        /// Renders type argments to the output stream if arguments exists.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="comments">Associated comments with the arguments that may exist.</param>
        /// <returns>true if content was rendered, false otherwise.</returns>
        public static bool PutTypeArgsWithComments(this MethodInfo info, MethodComments? comments)
        {
            if (!info.ContainsGenericParameters)
                return false;

            // Render each argment seperately
            foreach (var arg in info.GetGenericArguments())
            {
                // Render the type info
                AsMarkdown.UnorderedListItem.Prefix("@typeparam", AsMarkdown.Italic, Padding.Space);
                AsMarkdown.Code.Wrap(arg.Name);               
                
                // Get the comment text for the type arg if it exists
                string? text = comments?.TypeParameters.SingleOrDefault(p => p.Name.Equals(arg.Name)).Text;
                // Render the type arg if valid
                if (!string.IsNullOrWhiteSpace(text))
                {
                    AsGeneral.Comma.Prefix(text);
                }
                // Next iteration on a new line
                Padding.NewLine.Put();
            }
            return true;
        }

        /// <summary>
        /// Renders parameters to the output stream if they exists.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="comments">Associated comments with the parameters that may exist.</param>
        /// <returns>true if content was rendered, false otherwise.</returns>
        public static bool PutParametersWithComments(this MethodInfo info, MethodComments? comments)
        {
            var parameters = info.GetParameters();

            if (parameters.Length == 0)
                return false;

            parameters.ToMarkdown(each: (parameter, index) =>
            {
                AsMarkdown.UnorderedListItem.Prefix("@param", AsMarkdown.Italic, Padding.Space);

                parameter.ParameterType.MaybeLink(parameter.ParameterType.DeclaringType!, Padding.Space);

                AsMarkdown.BoldItalic.Wrap(parameter.Name);

                string? text = comments?.Parameters.SingleOrDefault(p => p.Name.Equals(parameter.Name)).Text;

                if (!string.IsNullOrWhiteSpace(text))
                {
                    AsGeneral.Comma.Put();
                    text.Put();
                }

                Padding.NewLine.Put();
            });

            return true;
        }

        /// <summary>
        /// Renders the return info to the output stream if info exists.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="comments">Associates comments with the return info that may exist.</param>
        /// <returns>true if content was rendered, false otherwise.</returns>
        public static bool PutReturnWithComments(this MethodInfo info, MethodComments? comments)
        {
            if (info.ReturnType.Name.Equals(typeof(void).Name))
                return false;

            AsMarkdown.UnorderedListItem.Prefix("@returns", AsMarkdown.Italic, Padding.Space);
            info.ReturnType.MaybeLink(info.ReturnType.DeclaringType);

            string? text = comments?.Returns;

            if (!string.IsNullOrWhiteSpace(text))
            {
                AsGeneral.Comma.Prefix(text);
            }

            Padding.NewLine.Put();

            return true;
        }

        #region Fetch Comments Helpers
        /// <summary>
        /// Helper function for getting a comment for a given <see cref="FieldInfo"/> instance.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool TryGetComments<T>(this MemberInfo info, [MaybeNullWhen(false)] out T? comments)
            where T : CommonComments
            => TryGetComments(info.MemberId, out comments);

        public static T? GetComments<T>(this MemberInfo info)
            where T : CommonComments
            => GetComments<T>(info.MemberId);

        /// <summary>
        /// Helper function for getting a comment for a given <see cref="Type"/> instance.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool TryGetComments(this Type info, [MaybeNullWhen(false)] out TypeComments comments)
            // Using MemberId on non-nested Classes is unsupported, hence we must:
            => TryGetComments(info.TypeId, out comments);

        /// <summary>
        /// Generic function containing core logic for attempting to retrieve comments if they exists, otherwise null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getter"></param>
        /// <returns></returns>
        static bool TryGetComments<T>(Func<string> getter, [MaybeNullWhen(false)] out T? comments)
            where T : CommonComments
        {
            var result = State.Comments!.TryGetValue(getter(), out var temp); // Cannot out directly without explicit cast
            comments = temp as T;
            return result;
        }

        static T? GetComments<T>(Func<string> getter)
            where T : CommonComments
        {
            _ = State.Comments!.TryGetValue(getter(), out var comments);
            return comments as T;
        }
        #endregion
    }
}
