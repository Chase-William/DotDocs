using DotDocs.Markdown.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using State = DotDocs.Markdown.RenderState;

namespace DotDocs.Markdown
{
    public static class TypeNaming
    {
        const int RANK_STARTING_INDEX = 1;

        static Type? origin;

        static Stack<int> arrRanks = new();

        static Stack<Type> types = new();

        /// <summary>
        /// Puts a given <see cref="Type"/>'s complete type to output.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="padding"></param>
        public static void PutTypeName(this Type to, Type? from = null, Padding padding = Padding.None)
        {
            origin = from; // Store origin state for routing
            to.PutTypeName(); // Begin recursively retrieving type names
            padding.Put();
            origin = null;
        }

        public static void PutTypeArgs(this MethodInfo method, Padding padding = Padding.None)
        {
            origin = method.DeclaringType;
            foreach (var to in method.GetGenericArguments())
                to.PutTypeName();
            padding.Put();
            origin = null;
        }

        #region Private Implem
        static void PutTypeName(this Type to)
        {
            types.Push(to);

            // If array, look into and get element type
            if (to.IsArray)
            {
                // Inspect element type in next recursive iteration
                // Must keep "to" as the array as we need to add the suffix once all recursive processing returns
                to.GetElementType()!.PutTypeName();
            }
            else // Not array, put root name
            { 
                to.PutMaybeLink();
            }

            // If type is generic, there must be generic type params or args present, otherwise skip
            if (to.IsGenericType)
            {
                if (to.GenericTypeArguments.Any()) // Process args if they exits
                    to.GenericTypeArguments.PutTypeArgs();
                else // Args didn't exists, therefore, params must
                    to.GetTypeInfo().GenericTypeParameters.PutTypeParams();
            }

            // When returning back up the call chain, if this was an array, store the rank in the stack state
            // A stack must be used for LIFO functionalitity, otherwise array ranks render in reverse
            if (to.IsArray)
                arrRanks.Push(to.GetArrayRank());

            // Pop this type apon returning
            types.Pop();

            // See if we have reached the end of the type chain
            if (types.TryPeek(out Type? next))
            {
                if (!next.IsArray) // If the next element is NOT an array, make sure previous array suffix get rendered
                    PutMaybeArraySuffix();
            }           
            else // If stack is empty, ensure that if the root type was an array itself, it's suffix gets rendered
                PutMaybeArraySuffix();
        }

        /// <summary>
        /// Renders the array suffix by clearing the <see cref="arrRanks"/> <see cref="Stack{T}"/> as it iterates.
        /// </summary>
        static void PutMaybeArraySuffix()
        {           
            int i = RANK_STARTING_INDEX;
            while (arrRanks.TryPop(out int rank))
            {
                "[ ".Put();
                for (; i < rank; i++)
                    AsGeneral.Comma.Put(Padding.Space);
                "]".Put();
                i = RANK_STARTING_INDEX;
            }            
        }

        static void PutTypeArgs(this IEnumerable<Type> args)
        {
            // - Type args can take type arguments
            // - Type args can take an element type
            // - Type args may be linkable to a user defined type
            // Therefore, call back recursively         
            args.ToMarkdown(
                before: delegate
                {
                    AsGeneral.SmallerThanArrow.Put();
                },
                each: (to, index) =>
                {
                    if (index > 0)
                        AsGeneral.Comma.Put(Padding.Space);
                    to.PutTypeName();
                },
                after: delegate
                {
                    AsGeneral.LargerThanArrow.Put();
                });
        }

        static void PutTypeParams(this IEnumerable<Type> _params)
        {
            // - Type Params cannot take type arguments
            // - Type Params cannot take an element type
            // - Type Params will never be linkable (their defined in a type's declaration)
            // Therefore, just the type as a code span
            _params.ToMarkdown(
                before: delegate
                {
                    AsGeneral.SmallerThanArrow.Put();
                },
                each: (to, index) =>
                {
                    if (index > 0)
                        AsGeneral.Comma.Put(Padding.Space);
                    AsMarkdown.Code.Wrap(to.Name);
                },
                after: delegate
                {
                    AsGeneral.LargerThanArrow.Put();
                });
        }

        static void PutMaybeLink(this Type to)
        {
            string name = to.Name;

            // Remove arg/param count label from type's name if generic
            if (to.IsGenericType)
                name = new string(to.Name.TakeWhile(static c => c != '`').ToArray());

            // Check other states of "to" and "origin" for linking validity      
            if (origin is not null &&
                !to.IsGenericParameter && // Cannot be a generic type parameter (dest wont exists)
                // !origin.IsGenericParameter && // ^
                to.Name != origin.Name && // Do not link to one's self
                State.Assemblies!.ContainsKey(to.Assembly.FullName!)) // Ensure this type was defined in the user's projects otherwise a destination won't exist
            {
                // Render link
                var path = State.Output.Router.GetRoute(origin, to);
                AsMarkdown.Link.Link(name, path, AsMarkdown.Code);
                return;
            }
            // Render normal code span for type representation
            AsMarkdown.Code.Wrap(name);
        }
        #endregion
    }
}
