using LoxSmoke.DocXml;
using System.Reflection;

namespace Docshark.Core.Models.Codebase.Members
{
    public class MemberModel<T1, T2> : Model<T1, T2>
        where T1 : MemberInfo
        where T2 : CommonComments
    {        
        public TypeKey Type { get; private set; }

        protected MemberModel(T1 member) : base(member) { }
        
        protected void SetType(Type info)
        {
            Type = new TypeKey
            {
                ForeignKey = info.GetPrimaryKey()
            };

            // If the type is generic, mark the typekey as generic
            if (info.IsGenericTypeDefinition)
                Type.IsGeneric = true;            
        }
    }
}
