using System.Reflection;

using LoxSmoke.DocXml;

namespace Charp.Core.Models.Members
{
    public class EventModel : Model<EventInfo, CommonComments>, IMemberable
    {
        public override string Type => Meta.EventHandlerType?.ToString();

        public bool IsVirtual => ((Meta.AddMethod?.IsVirtual ?? false) || (Meta.RemoveMethod?.IsVirtual ?? false)) && !IsAbstract;        
        public bool IsAbstract => (Meta.AddMethod?.IsAbstract ?? false) || (Meta.RemoveMethod?.IsAbstract ?? false);
        public bool IsStatic => (Meta.AddMethod?.IsStatic ?? false) || (Meta.RemoveMethod?.IsStatic ?? false);

        #region IMemberable
        public bool IsPublic => (Meta.AddMethod?.IsPublic ?? true) || (Meta.RemoveMethod?.IsPublic ?? true);
        public bool IsProtected => (Meta.AddMethod?.IsFamily ?? true) || (Meta.RemoveMethod?.IsFamilyOrAssembly ?? true);
        public bool IsInternal => (Meta.AddMethod?.IsAssembly ?? true) || (Meta.RemoveMethod?.IsFamilyOrAssembly ?? true);
        #endregion

        public EventModel(EventInfo member) : base(member) { }
    }
}
