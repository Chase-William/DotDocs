using System.Reflection;

using Charp.Core.Models.Members;
using LoxSmoke.DocXml;

namespace Charp.Core.Models.Types
{
    /// <summary>
    /// Represents an interface.
    /// </summary>
    public class InterfaceModel : TypeMember<TypeInfo, TypeComments>, IMemberContainable
    {
        /// <summary>
        /// Identifer for interface used in json parsers.
        /// </summary>
        public const string INTERFACE_TYPE_STRING = "interface";

        /// <summary>
        /// Denotes if this type can have internal types.
        /// </summary>
        public override bool CanHaveInternalTypes => true;

        /// <summary>
        /// Identifier for determining the type used in json parsers. 
        /// </summary>
        public override string Type => INTERFACE_TYPE_STRING;

        /// <summary>
        /// Properties of this interface.
        /// </summary>
        public PropertyModel[] Properties { get; set; }
        /// <summary>
        /// Fields of this interface.
        /// </summary>
        public FieldModel[] Fields { get; set; }
        /// <summary>
        /// Methods of this interface.
        /// </summary>
        public MethodModel[] Methods { get; set; }
        /// <summary>
        /// Events of this interface.
        /// </summary>
        public EventModel[] Events { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="InterfaceModel"/>.
        /// </summary>
        /// <param name="member">Information about this interface.</param>
        /// <param name="reader">A reader used to acquire written documentation.</param>
        public InterfaceModel(TypeInfo member, DocXmlReader reader) : base(member)
            => IMemberContainable.Init(this, member, reader);        
    }
}
