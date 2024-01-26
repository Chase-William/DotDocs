using DotDocs.IO;
using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown
{
    public static class RenderState
    {
        // The following state makes this class operate as a state machine reducing the times we must reference the StringBuilder when rendering
        #region State
        public static StringBuilder? Builder { get; private set; }
        public static ImmutableDictionary<string, Assembly>? Assemblies { get; private set; }
        public static ImmutableDictionary<string, CommonComments>? Comments { get; private set; }
        public static IOutputable? Output { get; private set; }
        #endregion

        public static void UpdateState(
            StringBuilder builder, 
            ImmutableDictionary<string, Assembly> assemblies, 
            ImmutableDictionary<string, CommonComments> comments,
            IOutputable output)
        {
            Builder = builder;
            Assemblies = assemblies;
            Comments = comments;
            Output = output;
        }

        /// <summary>
        /// Helper function for getting a comment for a given <see cref="FieldInfo"/> instance.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool TryGetComments<T>(this MemberInfo info, [MaybeNullWhen(false)] out T comments)
            where T : CommonComments
            => GetComments(info.MemberId, out comments);


        /// <summary>
        /// Helper function for getting a comment for a given <see cref="Type"/> instance.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool TryGetComments(this Type info, [MaybeNullWhen(false)] out TypeComments comments)
            => GetComments(info.TypeId, out comments);

        /// <summary>
        /// Generic function containing core logic for attempting to retrieve comments if they exists, otherwise null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getter"></param>
        /// <returns></returns>
        static bool GetComments<T>(Func<string> getter, [MaybeNullWhen(false)] out T comments)
            where T : CommonComments
        {            
            var result = Comments.TryGetValue(getter(), out var temp); // Cannot out directly without explicit cast
            comments = temp as T;
            return result;
        }    
    }
}
