using System.Reflection;

using Charp.Core.Models.Members;
using LoxSmoke.DocXml;

namespace Charp.Core.Models.Types
{
    /// <summary>
    /// Represents a enum.
    /// </summary>
    public class EnumModel : TypeMember<TypeInfo, TypeComments>, IFieldable
    {
        /// <summary>
        /// Identifier for determining type used by json parsers.
        /// </summary>
        public const string ENUM_TYPE_STRING = "enum";
        /// <summary>
        /// Denotes if this type can have internal types.
        /// </summary>
        public override bool CanHaveInternalTypes => false;
        /// <summary>
        /// Identifier for determining the type used by json parsers.
        /// </summary>
        public override string Type => ENUM_TYPE_STRING;

        /// <summary>
        /// Values of this enum as fields.
        /// </summary>
        public FieldModel[] Fields { get; set; }

        /// <summary>
        /// The underlying datatype used to represent each case.
        /// </summary>
        public string UnderlyingType => Meta.GetEnumUnderlyingType().ToString();        

        /// <summary>
        /// Initializes a new instance of <see cref="EnumModel"/>.
        /// </summary>
        /// <param name="member">Information about the enum.</param>
        /// <param name="reader">Reader used to acquire member information from.</param>
        public EnumModel(TypeInfo member, DocXmlReader reader) : base(member)
        {
            // Omit first the element as it is provided by default by the compiler
            var fields = member.GetFields();
            Fields = new FieldModel[fields.Length - 1];
            for (int i = 1; i < fields.Length; i++)
                Fields[i - 1] = new FieldModel(fields[i])
                {
                    Comments = reader.GetMemberComments(fields[i])
                };
        }
    }
}
