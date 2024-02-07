using DocXml.Reflection;
using DotDocs.IO;
using DotDocs.Markdown.Enums;
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

        //static string GetNameWithoutSpecifiers(this Type type)
        //{
        //    string name = type.Name;
                        
        //    // Remove from generics specifier onward
        //    if (type.IsGenericType || 
        //        ContainsGenericElementType(type)  // Check if type contains a generic element type recursively
        //    ) {
        //        name = new string(type.Name.TakeWhile(static c => c != '`').ToArray());
        //    }
        //    else if (type.HasElementType) // Check if non generic element type exists atleast
        //    {
        //        name = new string(type.Name.TakeWhile(static c => c != '[').ToArray());
        //    }

        //    // Returns true if any element types possibly present contain
        //    static bool ContainsGenericElementType(Type type)
        //    {
        //        // If no elements types left to check or element type is generic, return
        //        if (!type.HasElementType || type.IsGenericType)                
        //            return type.IsGenericType;                                   
        //        // Continue looping recursively
        //        return ContainsGenericElementType(type.GetElementType()!);
        //    }

        //    return name;            
        //}

        //static void PutArraySuffix(this Type type)
        //{
        //    if (type.IsArray)
        //    {
        //        type.Name[type.Name.IndexOf('[')..].Put();
        //    }            
        //}

        ///// <summary>
        ///// Checks and returns a <see cref="Type"/> instance if <see cref="Type.GetElementType"/> eventually returns a non-array based type recursively.
        ///// </summary>
        ///// <param name="typeSrc"></param>
        ///// <param name="type"></param>
        ///// <returns>True of non array based type was found, otherwise false.</returns>
        //static bool TryGetNonArrayElementType(this Type typeSrc, [NotNullWhen(true)] out Type? type)
        //{
        //    if (typeSrc.HasElementType)
        //        return TryGetNonArrayElementType(typeSrc.GetElementType()!, out type);
        //    type = typeSrc.IsArray ? null : typeSrc;
        //    return !typeSrc.IsArray;
        //}

        ///// <summary>
        ///// Render a markdown style hyper-link to the <paramref name="to"/> <see cref="Type"/> from the <paramref name="from"/> <see cref="Type"/>, otherwise if not possible, render as default.
        ///// </summary>
        ///// <param name="to"></param>
        ///// <param name="from"></param>
        ///// <param name="padding"></param>
        //static void MaybeLink(this Type to, Type from, Padding padding = Padding.None)
        //{
        //    string name;

        //    // Non-generic types don't need to be preprocessed to remove the type parameterized label
        //    name = to.GetNameWithoutSpecifiers();
            
        //    // Attempt to get a non-array element type from the current "to" type in an attempt to make a link to it
        //    // Linking to the constructed array type is an error, destination will not exist
        //    if (TryGetNonArrayElementType(to, out Type? eleType))
        //    {
        //        ArgumentNullException.ThrowIfNull(eleType);
        //        to = eleType;
        //    }

        //    // Ensure this is a routable type within the user's defined libraries and isn't any kind of generic type parameter
        //    // If all good, render a markdown hyper-link
        //    if (!to.IsGenericParameter && // Do not link if either to or from is a generic param
        //        !from.IsGenericParameter &&
        //        to.Name != from.Name && // Do not link to one's self
        //        State.Assemblies.ContainsKey(to.Assembly.FullName)) // Ensure this type was defined in the user's projects otherwise a destination won't exist
        //    {

        //        var path = State.Output.Router.GetRoute(from, to);
        //        AsMarkdown.Link.Link(name, path, AsMarkdown.Code, padding);
        //        return;
        //    }

        //    AsMarkdown.Code.Wrap(name, padding);
        //}

        ///// <summary>
        ///// Writes a type's name to output with generic type params/args if they exists.
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="padding"></param>
        //public static void PutTypeName(this Type type, Padding padding = Padding.None)
        //{
        //    type.PutTypeName(null);
        //    padding.Put();
        //}

        ///// <summary>
        ///// Writes a type's name to output with generics type params/args if they exist.
        ///// </summary>
        ///// <remarks>
        ///// Providing an instance for the <paramref name="from"/> will allow links to be created where possible.
        ///// </remarks>
        ///// <param name="type">The <see cref="Type"/> to have its name written to output.</param>
        ///// <param name="from">A <see cref="Type"/> that is publically defined in a user's assemblies.</param>
        ///// <param name="padding"></param>
        //public static void PutTypeName(this Type type, Type? from, Padding padding = Padding.None)
        //{            
        //    type.PutTypeName(from);
        //    type.PutArraySuffix();
        //    padding.Put();
        //}

        //static void PutTypeName(this Type type, Type? from)
        //{
        //    // Handle both cases (types never have both)
        //    if (from is null)
        //        AsMarkdown.Code.Wrap(type.GetNameWithoutSpecifiers());
        //    else
        //        type.MaybeLink(from);

        //    // If any element type exists, set it as our target type
        //    // The element type will house generic type params/args if they exists, not the array
        //    if (type.HasElementType && type.IsArray)
        //        type = type.GetElementType()!;

        //    // If type is generic, there must be generic type params or args present
        //    if (type.IsGenericType)
        //    {
        //        if (type.GenericTypeArguments.Any()) // Process args if they exits
        //            type.GenericTypeArguments.PutGenericTypeArguments(from);
        //        else // Args didn't exists, therefore, params must
        //            type.GetTypeInfo().GenericTypeParameters.PutGenericTypeParameters();
        //    }            
        //}

        ///// <summary>
        ///// Render type parameters.
        ///// </summary>
        ///// <remarks>
        ///// Type parameters will never be contructed from other type parameters and will never be defined else where they were declared from. Meaning, a type parameter will never need recursive processing and will never be linkable.
        ///// </remarks>
        ///// <param name="types"></param>
        //static void PutGenericTypeParameters(this IEnumerable<Type> types)
        //{
        //    if (!types.Any())
        //        return;

        //    AsGeneral.SmallerThanArrow.Put();
        //    types.ToMarkdown(each: (type, index) =>
        //    {
        //        if (index != 0)
        //            AsGeneral.Comma.Put(Padding.Space);
        //        // Render type parameter               
        //        AsMarkdown.Code.Wrap(type.Name);
        //    });
        //    AsGeneral.LargerThanArrow.Put();
        //}        

        //static void PutGenericTypeArguments(this IEnumerable<Type> types, Type from)
        //{
        //    if (!types.Any())
        //        return;

        //    AsGeneral.SmallerThanArrow.Put();
        //    types.ToMarkdown(each: (type, index) =>
        //    {
        //        if (index != 0)
        //            AsGeneral.Comma.Put(Padding.Space);
        //        // Render type parameter               
        //        type.PutTypeName(from);
        //    });
        //    AsGeneral.LargerThanArrow.Put();
        //}

        //public static void PutTypeArgs(this MethodInfo info)
        //    => PutGenericTypeParameters(info.GetGenericArguments());

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