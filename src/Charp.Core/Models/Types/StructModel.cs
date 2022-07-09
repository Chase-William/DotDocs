using System.Reflection;

using Charp.Core.Models.Members;
using LoxSmoke.DocXml;

namespace Charp.Core.Models.Types
{
    /// <summary>
    /// Represents a struct.
    /// </summary>
    public class StructModel : TypeMember<TypeInfo, TypeComments>, IMemberContainable
    {
        /// <summary>
        /// Identifier for determining type used by json parsers.
        /// </summary>
        public const string STRUCT_TYPE_STRING = "struct";

        /// <summary>
        /// Denotes if this type can have internal types.
        /// </summary>
        public override bool CanHaveInternalTypes => true;

        /// <summary>
        /// Identifier for determining the type used by json parsers.
        /// </summary>
        public override string Type => STRUCT_TYPE_STRING;

        /// <summary>
        /// Properties of this struct.
        /// </summary>
        public PropertyModel[] Properties { get; set; }
        /// <summary>
        /// Fields of this struct.
        /// </summary>
        public FieldModel[] Fields { get; set; }
        /// <summary>
        /// Methods of this class.
        /// </summary>
        public MethodModel[] Methods { get; set; }
        /// <summary>
        /// Events of this struct.
        /// </summary>
        public EventModel[] Events { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="StructModel"/>.
        /// </summary>
        /// <param name="member">Information about the struct.</param>
        /// <param name="reader">A reader to acquire written documentation from.</param>
        public StructModel(TypeInfo member, DocXmlReader reader) : base(member)
            => IMemberContainable.Init(this, member, reader);        
    }
}
