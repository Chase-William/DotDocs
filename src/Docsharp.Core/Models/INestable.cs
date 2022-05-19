using System;
using System.Linq;
using System.Reflection;

using Docsharp.Core.Models.Members;

namespace Docsharp.Core.Models
{
    /// <summary>
    /// Represents a type that can contain Properties, Fields, and Methods.
    /// </summary>
    public interface INestable : IFieldable
    {
        public PropertyModel[] Properties { get; set; }
        
        public MethodModel[] Methods { get; set; }

        public EventModel[] Events { get; set; }

        public static void Init(INestable constructable, TypeInfo info)
        {
            constructable.Properties = constructable.GetProperties(info);
            constructable.Fields = constructable.GetFields(info);
            constructable.Methods = constructable.GetMethods(info);
            constructable.Events = constructable.GetEvents(info);
        }

        public PropertyModel[] GetProperties(TypeInfo info)
        {
            var props = info.GetProperties();
            if (props.Length == 0)
                return Array.Empty<PropertyModel>();
            var tempProps = new PropertyModel[props.Length];
            for (int i = 0; i < props.Length; i++)
                tempProps[i] = new PropertyModel(props[i]);
            return tempProps;
        }        

        public MethodModel[] GetMethods(TypeInfo info)
        {
            var methods = info.GetMethods().Where(method => !method.IsSpecialName);            
            int length = methods.Count();
            if (length == 0)
                return Array.Empty<MethodModel>();
            var tempMethods = new MethodModel[length];
            length = 0; // reset length as index in loop below
            foreach (var method in methods)
                tempMethods[length++] = new MethodModel(method);
            return tempMethods;
        }

        public EventModel[] GetEvents(TypeInfo info)
        {
            var events = info.GetEvents();
            int length = events.Count();
            if (length == 0)
                return Array.Empty<EventModel>();
            var tempEvents = new EventModel[length];
            length = 0;
            foreach (var method in events)
                tempEvents[length++] = new EventModel(method);
            return tempEvents;
        }
    }
}
