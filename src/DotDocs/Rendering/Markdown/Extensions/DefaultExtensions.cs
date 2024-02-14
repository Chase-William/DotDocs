using DocXml.Reflection;
using DotDocs.IO;
using DotDocs.Markdown.Enums;
using DotDocs.Markdown.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using State = DotDocs.Markdown.RenderState;

namespace DotDocs.Markdown.Extensions
{
    /// <summary>
    /// A static class containing extensions methods for writing to the global <see cref="StringBuilder"/> state.
    /// </summary>
    public static class DefaultExtensions
    {
        #region Put Markdown Functions
        public static void Put(this string str, Padding padding = Padding.None)
        {
            State.Builder.Append(str);
            padding.Put();
        }

        public static void Put(this char character, Padding padding = Padding.None)
        {
            State.Builder.Append(character);
            padding.Put();
        }

        public static void ToMarkdown<T>(
            this IEnumerable<T> items,
            Action? before = null,
            Action<T, int>? each = null,
            Action? after = null)
        {
            // If empty, skip all rendering
            if (!items.Any())
                return;

            before?.Invoke();
            if (each is not null)
            {
                int index = 0;
                foreach (var item in items)
                    each(item, index++);
            }
            after?.Invoke();
        }
        #endregion
    }
}