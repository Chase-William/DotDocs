using System.Reflection;

using Charp.Core.Models.Members;
using LoxSmoke.DocXml;

namespace Charp.Core.Models.Types
{
    public class InterfaceModel : TypeMember<TypeInfo, TypeComments>, INestable
    {
        public const string INTERFACE_TYPE_STRING = "interface";
        public override bool CanHaveInternalTypes => true;
        public override string Type => INTERFACE_TYPE_STRING;

        public PropertyModel[] Properties { get; set; }
        public FieldModel[] Fields { get; set; }
        public MethodModel[] Methods { get; set; }
        public EventModel[] Events { get; set; }

        public InterfaceModel(TypeInfo member, DocXmlReader reader) : base(member)
        {
            INestable.Init(this, member, reader);
        }
    }
}
