using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models.Language.Members
{
    public class EventModel : MemberModel
    {
        public string Name { get; set; }

        public ITypeable EventHandlerType { get; set; }

        public EventModel(
            EventInfo info,
            ImmutableDictionary<string, AssemblyModel> assemblies,
            Dictionary<string, ITypeable> types
            ) {
            Name = info.Name;            
            // info.EventHandlerType.
            EventHandlerType = ITypeable.GetOrCreateTypeFrom(info.EventHandlerType, assemblies, types);
        }
    }
}
