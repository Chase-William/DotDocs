using DotDocs.Markdown.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown.Renderers.Members
{
    public interface IMemberRenderer
    {
        public static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();              
        
        /// <summary>
        /// Renders a single instance of a <see cref="MemberInfo"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        public void Render<T>(T info) where T : MemberInfo;
    }

    public static class CastCheckingEx
    {
        public static bool Check<TMember>(
            this MemberInfo actual, 
            [NotNullWhen(false)] out ArgumentException ex,
            [NotNullWhen(true)] out TMember info)
            where TMember : MemberInfo
            => Check(
                actual, 
                out ex!, 
                out info!, 
                message: $"Argument member {actual} was expected to be of type {typeof(TMember)}");

        /// <summary>
        /// Checks to see if <typeparamref name="TMember"/> matches the type of <paramref name="actual"/> returning true if yes, false if otherwise. When the check fails, an exception is created of type <typeparamref name="TExcept"/> using the optional <paramref name="message"/> as a argument and then assigned to <paramref name="ex"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="MemberInfo"/> type to compared against is infered from the <typeparamref name="TMember"/>.
        /// </remarks>
        /// <typeparam name="TMember">The expected <see cref="MemberInfo"/> type derivative.</typeparam>
        /// <typeparam name="TExcept">The exception type to be created.</typeparam>
        /// <param name="actual">A reference to the actual <see cref="MemberInfo"/> instance.</param>
        /// <param name="ex">A reference to the exception instance created if false.</param>
        /// <param name="info">A reference to the casted version of the <paramref name="actual"/> if true.</param>
        /// <param name="message">An optional message to be provided to the exception constructor.</param>
        /// <returns>True if types match, false if otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <see cref="Activator"/> fails to create an instance of <typeparamref name="TExcept"/>.</exception>
        public static bool Check<TMember, TExcept>(
            this MemberInfo actual, 
            [NotNullWhen(false)] out TExcept? ex, 
            [NotNullWhen(true)] out TMember? info,
            string message = "") 
            where TMember : MemberInfo
            where TExcept : Exception, new()
        {
            if (actual is TMember _info)
            {
                ex = null;
                info = _info;
                return true;
            }
            //$"Argument member {actual} was expected to be of type {typeof(TMember)}"
            ex = Activator.CreateInstance(typeof(TExcept), new object[] { message }) as TExcept ?? throw new ArgumentNullException($"Failed to create an instance of {typeof(TExcept)}");           
            info = null;
            return false;
        }
    }
}
