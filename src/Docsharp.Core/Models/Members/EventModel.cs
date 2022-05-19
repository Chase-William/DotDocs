using System.Reflection;

using Docsharp.Core.Models.Docs;

namespace Docsharp.Core.Models.Members
{
    public class EventModel : Member<EventInfo, Documentation>
    {
        public override string Type => "event";

        public EventModel(EventInfo member) : base(member) { }
    }
}
