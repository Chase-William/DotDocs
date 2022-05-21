using System.Reflection;

using LoxSmoke.DocXml;

namespace Docsharp.Core.Models.Members
{
    public class EventModel : Member<EventInfo, CommonComments>
    {
        public override string Type => "event";

        public EventModel(EventInfo member) : base(member) { }
    }
}
