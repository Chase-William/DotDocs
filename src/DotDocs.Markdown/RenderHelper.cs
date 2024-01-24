using DotDocs.IO;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown
{
    public readonly struct Markdown
    {
        public const string DEFAULT_SPACING = "\n\n";
    }

    public enum HeaderVariant : byte
    {
        H1 = 1,
        H2,
        H3,
        H4,
        H5
    }

    public enum Padding
    {
        Default,
        NoPadding
    }

    public static class RenderHelper
    {
        // The following state makes this class operate as a state machine reducing the times we must reference the StringBuilder when rendering
        #region State
        public static StringBuilder Builder { get; set; }
        public static ImmutableDictionary<string, Assembly> Assemblies { get; set; }
        public static IOutputable Output { get; set; }
        #endregion

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

        public static string AsMarkdownParams(this IEnumerable<ParameterInfo> _params)
          => $"({string.Join(", ", _params.Select(p => $"{p.ParameterType.AsMaybeLink()} {p.Name.AsItalic()}"))})";

        public static string AsMaybeLink(this Type type)
        {
            // Return a link to documented type if it was defined in one of the user's project assemblies
            if (Assemblies.ContainsKey(type.Assembly.GetKey()))
                return type.Name
                    .AsCode()
                    .AsLink("./" + type.Name); // ----------------------------------------- Need to create relative link, from: Type -> to: Type
            return type.Name.AsCode();
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

        public static string AsNameWithoutGenericInfo(this string str)
            => new(str.TakeWhile(c => c != '`').ToArray());
        #endregion

        #region Put Markdown Functions
        public static void Put(this string str)
           => Builder.Append(str);

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

            Builder.Append(temp);
            Builder.Append(str);
            if (padded)
                Markdown.DEFAULT_SPACING.Put();
        }

        /// <summary>
        /// Iterates over a collection of models calling the given render function on each one.
        /// After each callback, default line padding is added to the provided <see cref="StringBuilder"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="models"></param>
        /// <param name="builder"></param>
        /// <param name="render"></param>
        public static void ToMarkdown<T>(
            this IEnumerable<T> models,
            Func<T, Padding> render
            )
        {
            foreach (var model in models)
                if (render(model) == Padding.Default)
                    Markdown.DEFAULT_SPACING.Put();
        }
        #endregion
    }
}