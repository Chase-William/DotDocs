using System.Reflection;
using DotDocs.Core.Models.Comments;
using LoxSmoke.DocXml;

namespace DotDocs.Core.Models.Language.Members
{
    public class EventModel : MemberModel<EventInfo, CommonCommentsModel<CommonComments>>
    {

        public bool IsVirtual => ((Info.AddMethod?.IsVirtual ?? false) || (Info.RemoveMethod?.IsVirtual ?? false)) && !IsAbstract;
        public bool IsAbstract => (Info.AddMethod?.IsAbstract ?? false) || (Info.RemoveMethod?.IsAbstract ?? false);
        public bool IsStatic => (Info.AddMethod?.IsStatic ?? false) || (Info.RemoveMethod?.IsStatic ?? false);

        #region Accessbility
        public bool IsPublic => (Info.AddMethod?.IsPublic ?? true) || (Info.RemoveMethod?.IsPublic ?? true);
        public bool IsPrivate => (Info.AddMethod?.IsPrivate ?? true) || (Info.RemoveMethod?.IsPrivate ?? true);
        public bool IsProtected => (Info.AddMethod?.IsFamily ?? true) || (Info.RemoveMethod?.IsFamilyOrAssembly ?? true);
        public bool IsInternal => (Info.AddMethod?.IsAssembly ?? true) || (Info.RemoveMethod?.IsFamilyOrAssembly ?? true);
        #endregion

        public EventModel(EventInfo member) : base(member) { }
    }
}
