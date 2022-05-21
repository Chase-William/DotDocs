using System.Reflection;

using Charp.Core.Models.Members;
using LoxSmoke.DocXml;

namespace Charp.Core.Models.Types
{
    public class EnumModel : TypeMember<TypeInfo, TypeComments>, IFieldable
    {
        public const string ENUM_TYPE_STRING = "enum";
        public override bool CanHaveInternalTypes => false;
        public override string Type => ENUM_TYPE_STRING;

        public FieldModel[] Fields { get; set; }

        public string UnderLyingType => Meta.GetEnumUnderlyingType().ToString();        

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
