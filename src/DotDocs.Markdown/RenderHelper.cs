using DocXml.Reflection;
using DotDocs.IO;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using State = DotDocs.Markdown.RenderState;

namespace DotDocs.Markdown
{
    public readonly struct Markdown
    {
        public const string NEW_LINE = "\n";
        public const string DOUBLE_NEW_LINE = "\n\n";        
    }   

    public static class RenderHelper
    {
        // The following state makes this class operate as a state machine reducing the times we must reference the StringBuilder when rendering
        //#region State
        //public static StringBuilder Builder { get; set; }
        //public static ImmutableDictionary<string, Assembly> Assemblies { get; set; }
        //public static IOutputable Output { get; set; }
        //#endregion

        #region Get As Functions
        public static string AsItalic(this string str)
            => $"*{str}*";

        public static string AsBold(this string str)
            => $"**{str}**";

        public static string AsBoldItalic(this string str)
            => $"***{str}***";

        public static string AsCode(this string str)
            => $"`{str}`";             

        public static string AsLink(this string str, string href)
            => $"[{str}]({href})";

        public static string AsListItem(this string str, ListStyle style = ListStyle.Unordered)
            => $"{(style == ListStyle.Ordered ? "1." : "-")} {str}";

        public static string AsMarkdownParams(this IEnumerable<ParameterInfo> _params)
          => $"({string.Join(", ", _params.Select(p => $"{p.ParameterType.AsMaybeLink()} {p.Name.AsItalic()}"))})";

        public static string AsMaybeLink(this Type type)
        {
            // Return a link to documented type if it was defined in one of the user's project assemblies
            if (State.Assemblies.ContainsKey(type.Assembly.FullName))
                return type
                    .ToNameString()
                    .AsCode()
                    .AsLink("./" + type.Name); // ----------------------------------------- Need to create relative link, from: Type -> to: Type
            return type.ToNameString().AsCode();
        }             

        // Use Type.GetNameString();
        //public static string AsGenericTypeArguments(this IEnumerable<Type> args)
        //{
        //    if (args.Count() == 0)
        //        return string.Empty;

        //    var str = "<";
        //    str += string.Join(
        //        ", ", 
        //        args.Select(t => t.Name.AsNameWithoutGenericInfo() +
        //        t.GenericTypeArguments.AsGenericTypeArguments()));
        //    return str += ">";
        //}

        //public static string AsNameWithoutGenericInfo(this string str)
        //    => new(str.TakeWhile(c => c != '`').ToArray());
        #endregion

        #region Put Markdown Functions
        public static void Put(this string str)
           => State.Builder.Append(str);

        //public static void PutTypeName(this Type type)        
        //    => State.Builder.Append(type.ToNameString());             

        public static void PutMarkdownHeader(
            this string str,
            HeaderVariant variant,
            bool padded = true
            ) {
            // + 1 for space character
            var temp = new char[(int)variant + 1];
            byte i = 0;
            for (; i < (byte)variant; i++)
                temp[i] = '#';
            temp[i++] = ' ';

            State.Builder.Append(temp);
            State.Builder.Append(str);
            if (padded)
                Markdown.DOUBLE_NEW_LINE.Put();
        }

        /// <summary>
        /// Iterates over a collection of models calling the given render function on each one. After each callback, default line padding is added to the provided <see cref="StringBuilder"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="models"></param>
        /// <param name="before">Executes once before <paramref name="each"/></param>
        /// <param name="each">Executes for every <typeparamref name="T"/> in <paramref name="models"/>.</param>
        /// <param name="after">Executes once after everything else.</param>
        public static void ToMarkdown<T>(
            this IEnumerable<T> models,
            Action? before = null,
            Action<T>? each = null,            
            Action? after = null)
            where T : MemberInfo
        {
            // If empty, skip all rendering
            if (!models.Any())
                return;

            before?.Invoke();
            if (each is not null)
                foreach (var model in models)
                    each(model);                        
            after?.Invoke();
        }
        #endregion
    }
}