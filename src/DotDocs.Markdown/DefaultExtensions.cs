using DocXml.Reflection;
using DotDocs.IO;
using DotDocs.Markdown.Enums;
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
    public static class DefaultExtensions
    {
        #region Put Markdown Functions
        public static void Put(this string str, Padding padding = Padding.None)
        {            
            State.Builder.Append(str);
            padding.Put();
        }        

        /// <summary>
        /// Render a markdown style hyper-link to the <paramref name="to"/> <see cref="Type"/> from the <paramref name="from"/> <see cref="Type"/>, otherwise if not possible, render as default.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="padding"></param>
        public static void MaybeLink(this Type to, Type from, Padding padding = Padding.None)
        {
            // Always route from the declaring type
            // If using a nested type, does declaring type always work?

            // Ensure this is a routable type within the user's defined libraries and isn't any kind of generic type parameter
            // If all good, render a markdown hyper-link
            if (!to.IsGenericParameter && State.Assemblies.ContainsKey(to.Assembly.FullName))
            {
                var path = State.Output.Router.GetRoute(from, to);
                AsMarkdown.Link.Link(to.Name, path, AsMarkdown.Code, padding);
                return;
            }

            AsMarkdown.Code.Wrap(to.Name, padding);
        }

        /// <summary>
        /// Writes type arguments recursively to the output.
        /// </summary>
        /// <param name="args">Arguments of current <see cref="Type"/> or <see cref="MethodInfo"/>.</param>
        /// <param name="writer">Called when writing to output for type argument should occur. Allows for formating options for a given <see cref="Type"/> and its name.</param>
        public static void PutTypeArguments(this IEnumerable<Type> args, Action<Type> writer)
        {
            // Return if params is empty before writing anything to output
            if (!args.Any())
                return;

            AsGeneral.SmallerThanArrow.Put();

            args.ToMarkdown(each: (type, index) =>
            {
                if (index != 0)
                    AsGeneral.Comma.Put();
                // Render type, then check recursively if it too defines type parameters
                writer(type);                
                
                // Check and process possible recursive type params
                type.GetGenericArguments().PutTypeArguments(writer);
            });

            AsGeneral.LargerThanArrow.Put();
        }

        /// <summary>
        /// Iterates over a collection of models calling the given render function on each one. After each callback, default line padding is added to the provided <see cref="StringBuilder"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="before">Executes once before <paramref name="each"/></param>
        /// <param name="each">Executes for every <typeparamref name="T"/> in <paramref name="items"/>.</param>
        /// <param name="after">Executes once after everything else.</param>
        public static void ToMarkdown<T>(
            this IEnumerable<T> items,
            Action? before = null,
            Action<T>? each = null,            
            Action? after = null)
        {
            // If empty, skip all rendering
            if (!items.Any())
                return;

            before?.Invoke();
            if (each is not null)
                foreach (var item in items)
                    each(item);                        
            after?.Invoke();
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