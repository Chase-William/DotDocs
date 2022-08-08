using System.Collections.Generic;
using System.Reflection;
using Docshark.Core.Models.Codebase.Members;
using LoxSmoke.DocXml;

namespace Docshark.Core.Models.Codebase.Types
{
    /// <summary>
    /// Represents a class.
    /// </summary>
    public class ClassModel : TypeMember<TypeInfo, TypeComments>, IMemberContainable
    {
        /// <summary>
        /// Identifier for determining type used by json parsers.
        /// </summary>
        public const string CLASS_TYPE_STRING = "class";
        /// <summary>
        /// Denotes if this type can have internal types.
        /// </summary>
        public override bool CanHaveInternalTypes => true;
        /// <summary>
        /// Identifier for determining the type used by json parsers.
        /// </summary>
        public override string Type => CLASS_TYPE_STRING;

        /// <summary>
        /// Denotes if this class is sealed.
        /// </summary>
        public bool IsSealed => Meta.IsSealed;
        /// <summary>
        /// Denotes if this class is abstract.
        /// </summary>
        public bool IsAbstract => Meta.IsAbstract;
        /// <summary>
        /// Denotes if this class is static.
        /// </summary>
        public bool IsStatic => IsAbstract && IsSealed;

        /// <summary>
        /// Properties of this class.
        /// </summary>
        public PropertyModel[] Properties { get; set; }
        /// <summary>
        /// Fields of this class.
        /// </summary>
        public FieldModel[] Fields { get; set; }
        /// <summary>
        /// Methods of this class.
        /// </summary>
        public MethodModel[] Methods { get; set; }
        /// <summary>
        /// Events of this class.
        /// </summary>
        public EventModel[] Events { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ClassModel"/>.
        /// </summary>
        /// <param name="member">Information about this <see cref="ClassModel"/>.</param>
        /// <param name="reader">Reader used to acquire written documentation from.</param>
        public ClassModel(TypeInfo member, DocXmlReader reader) : base(member)
            => IMemberContainable.Init(this, member, reader);                    
    }
}
