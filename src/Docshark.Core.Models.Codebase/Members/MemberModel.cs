using LoxSmoke.DocXml;
using System.Reflection;

namespace Docshark.Core.Models.Codebase.Members
{
    public class MemberModel<T1, T2> : Model<T1, T2>
        where T1 : MemberInfo
        where T2 : CommonComments
    {        
        public TypeKey Type { get; protected set; }

        protected MemberModel(T1 member) : base(member) { }        
    }
}
