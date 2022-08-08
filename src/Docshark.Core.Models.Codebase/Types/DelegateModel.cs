using Docshark.Core.Models.Codebase;
using LoxSmoke.DocXml;
using System.Reflection;

namespace Docshark.Core.Models.Codebase.Types
{
    /// <summary>
    /// Represents a delegate.
    /// </summary>
    public class DelegateModel : TypeMember<TypeInfo, TypeComments>, IFunctional
    {
        /// <summary>
        /// Identifier for determining type used by json parsers.
        /// </summary>
        public const string DELEGATE_TYPE_STRING = "delegate";
        /// <summary>
        /// Denotes if this type can have internal types.
        /// </summary>
        public override bool CanHaveInternalTypes => false;
        /// <summary>
        /// Identifier for determining type used by json parsers.
        /// </summary>
        public override string Type => DELEGATE_TYPE_STRING;

        // public new FunctionalDocumentation Docs => (FunctionalDocumentation)base.Docs;

        /// <summary>
        /// Return type of this <see cref="DelegateModel"/> as a string.
        /// </summary>
        public string ReturnType { get; init; }
        /// <summary>
        /// Parameters expect of this <see cref="DelegateModel"/>.
        /// </summary>
        public Parameter[] Parameters { get; init; }

        /// <summary>
        /// Initializes a new instance of <see cref="DelegateModel"/>.
        /// </summary>
        /// <param name="member">Information about this <see cref="DelegateModel"/>.</param>
        public DelegateModel(TypeInfo member) : base(member)
        {
            var info = member.GetMethod("Invoke");
            ReturnType = info.ReturnType.ToString();
            Parameters = ((IFunctional)this).GetParameters(info);
        }
    }
}
