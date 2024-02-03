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

        public static void Put(this char character, Padding padding = Padding.None)
        {
            State.Builder.Append(character);
            padding.Put();
        }

        static string GetCleanedTypeName(this Type type)
            => new string(type.Name.TakeWhile(static c => c != '`').ToArray());

        /// <summary>
        /// Render a markdown style hyper-link to the <paramref name="to"/> <see cref="Type"/> from the <paramref name="from"/> <see cref="Type"/>, otherwise if not possible, render as default.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="padding"></param>
        static void MaybeLink(this Type to, Type from, Padding padding = Padding.None)
        {
            // Always route from the declaring type
            // If using a nested type, does declaring type always work?

            string name = string.Empty;

            // Non-generic types don't need to be preprocessed to remove the type parameterized label
            if (to.IsGenericType)
                name = to.GetCleanedTypeName();
            else
                name = to.Name;


            // Ensure this is a routable type within the user's defined libraries and isn't any kind of generic type parameter
            // If all good, render a markdown hyper-link
            if (to.Name != from.Name && // Do not link to one's self
                !to.IsGenericParameter &&
                !from.IsGenericParameter &&               
                State.Assemblies.ContainsKey(to.Assembly.FullName)) // Ensure this type was defined in the user's projects otherwise a destination won't exist
            {
                //if (to.FullName is null || from.FullName is null)
                //{
                //    Console.WriteLine();
                //}
                var path = State.Output.Router.GetRoute(from, to);
                AsMarkdown.Link.Link(name, path, AsMarkdown.Code, padding);
                return;
            }

            AsMarkdown.Code.Wrap(name, padding);
        }

        /// <summary>
        /// Writes a type's name to output with generic type params/args if they exists.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="padding"></param>
        public static void PutTypeName(this Type type, Padding padding = Padding.None)
        {
            type.PutTypeName(null);
            padding.Put();
        }

        /// <summary>
        /// Writes a type's name to output with generics type params/args if they exist.
        /// </summary>
        /// <remarks>
        /// Providing an instance for the <paramref name="from"/> will allow links to be created where possible.
        /// </remarks>
        /// <param name="type">The <see cref="Type"/> to have its name written to output.</param>
        /// <param name="from">A <see cref="Type"/> that is publically defined in a user's assemblies.</param>
        /// <param name="padding"></param>
        public static void PutTypeName(this Type type, Type? from, Padding padding = Padding.None)
        {            
            type.PutTypeName(from);
            padding.Put();
        }

        static void PutTypeName(this Type type, Type? from)
        {
            // Handle both cases (types never have both)
            if (from is null)
                AsMarkdown.Code.Wrap(type.GetCleanedTypeName());
            else
                type.MaybeLink(from);

            if (type.ContainsGenericParameters)
            {                               
                if (type.GenericTypeArguments.Any())
                    type.GenericTypeArguments.PutGenericTypeArguments(from);
                else
                    type.GetTypeInfo().GenericTypeParameters.PutGenericTypeParameters();
            }                
        }

        /// <summary>
        /// Render type parameters.
        /// </summary>
        /// <remarks>
        /// Type parameters will never be contructed from other type parameters and will never be defined else where they were declared from. Meaning, a type parameter will never need recursive processing and will never be linkable.
        /// </remarks>
        /// <param name="types"></param>
        static void PutGenericTypeParameters(this IEnumerable<Type> types)
        {
            if (!types.Any())
                return;

            AsGeneral.SmallerThanArrow.Put();
            types.ToMarkdown(each: (type, index) =>
            {
                if (index != 0)
                    AsGeneral.Comma.Put(Padding.Space);
                // Render type parameter               
                AsMarkdown.Code.Wrap(type.Name);
            });
            AsGeneral.LargerThanArrow.Put();
        }        

        static void PutGenericTypeArguments(this IEnumerable<Type> types, Type from)
        {
            if (!types.Any())
                return;

            AsGeneral.SmallerThanArrow.Put();
            types.ToMarkdown(each: (type, index) =>
            {
                if (index != 0)
                    AsGeneral.Comma.Put(Padding.Space);
                // Render type parameter               
                type.PutTypeName(from);
            });
            AsGeneral.LargerThanArrow.Put();
        }

        public static void PutTypeArgs(this MethodInfo info)
            => PutGenericTypeParameters(info.GetGenericArguments());

        //public static void PutTypeArguments(this MethodInfo info, Action<Type> writer)
        //    => info.GetGenericArguments().PutTypeArguments(writer);

        //public static void PutTypeParameters(this Type type, Action<Type> writer)
        //{
        //    type.GetTypeInfo().GenericTypeParameters.PutTypeParameters(writer);
        //}       

        //public static void PutTypeParameters(this IEnumerable<Type> parameters, Action<Type> writer)
        //{
        //    // Return if params is empty before writing anything to output
        //    if (!parameters.Any())
        //        return;

        //    AsGeneral.SmallerThanArrow.Put();

        //    parameters.ToMarkdown(each: (type, index) =>
        //    {
        //        if (index != 0)
        //            AsGeneral.Comma.Put();
        //        // Render type, then check recursively if it too defines type parameters
        //        writer(type);

        //        // if (type.parameter)
        //        // Check and process possible recursive type params
        //        type.GetTypeInfo().GenericTypeParameters.PutTypeArguments(writer);
        //    });

        //    AsGeneral.LargerThanArrow.Put();
        //}

        ///// <summary>
        ///// Writes type arguments recursively to the output.
        ///// </summary>
        ///// <param name="args">Arguments of current <see cref="Type"/> or <see cref="MethodInfo"/>.</param>
        ///// <param name="writer">Called when writing to output for type argument should occur. Allows for formating options for a given <see cref="Type"/> and its name.</param>
        //public static void PutTypeArguments(this IEnumerable<Type> args, Action<Type> writer)
        //{
        //    // Return if params is empty before writing anything to output
        //    if (!args.Any())
        //        return;

        //    AsGeneral.SmallerThanArrow.Put();

        //    args.ToMarkdown(each: (type, index) =>
        //    {
        //        if (index != 0)
        //            AsGeneral.Comma.Put();
        //        // Render type, then check recursively if it too defines type parameters
        //        writer(type);                

        //        // Check and process possible recursive type params
        //        type.GetGenericArguments().PutTypeArguments(writer);
        //    });

        //    AsGeneral.LargerThanArrow.Put();
        //}

        /// <summary>
        /// Iterates over a collection of models calling the given render function on each one. After each callback, default line padding is added to the provided <see cref="StringBuilder"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="before">Executes once before <paramref name="each"/></param>
        /// <param name="each">Executes for every <typeparamref name="T"/> in <paramref name="items"/>.</param>
        /// <param name="after">Executes once after everything else.</param>
        //public static void ToMarkdown<T>(
        //    this IEnumerable<T> items,
        //    Action? before = null,
        //    Action<T>? each = null,            
        //    Action? after = null)
        //{
        //    // If empty, skip all rendering
        //    if (!items.Any())
        //        return;

        //    before?.Invoke();
        //    if (each is not null)
        //        foreach (var item in items)
        //            each(item);                        
        //    after?.Invoke();
        //}

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