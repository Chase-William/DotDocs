using System.Reflection;

using LoxSmoke.DocXml;

namespace Charp.Core.Models.Members
{
    public class EventModel : Member<EventInfo, CommonComments>
    {
        public override string Type => Meta.EventHandlerType?.ToString();

        public bool IsVirtual => ((Meta.AddMethod?.IsVirtual ?? false) || (Meta.RemoveMethod?.IsVirtual ?? false)) && !IsAbstract;
        public bool IsPublic => (Meta.AddMethod?.IsPublic ?? false) || (Meta.RemoveMethod?.IsPublic ?? false);
        public bool IsAbstract => (Meta.AddMethod?.IsAbstract ?? false) || (Meta.RemoveMethod?.IsAbstract ?? false);
        public bool IsStatic => (Meta.AddMethod?.IsStatic ?? false) || (Meta.RemoveMethod?.IsStatic ?? false);

        public EventModel(EventInfo member) : base(member) { }
    }
}
