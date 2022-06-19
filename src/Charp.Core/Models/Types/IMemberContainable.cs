using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Charp.Core.Models.Members;

using LoxSmoke.DocXml;

namespace Charp.Core.Models.Types
{
    /// <summary>
    /// Represents a type that can contain Properties, Fields, and Methods.
    /// </summary>
    public interface IMemberContainable : IFieldable
    {
        public PropertyModel[] Properties { get; set; }
        
        public MethodModel[] Methods { get; set; }

        public EventModel[] Events { get; set; }      

        public static void Init(IMemberContainable constructable, TypeInfo info, DocXmlReader reader)
        {
            constructable.Properties = constructable.GetProperties(info, reader);
            constructable.Fields = constructable.GetFields(info, reader);
            constructable.Methods = constructable.GetMethods(info, reader);
            constructable.Events = constructable.GetEvents(info, reader);
        }

        public PropertyModel[] GetProperties(TypeInfo info, DocXmlReader reader)
        {
            // Need to call GetRuntime because otherwise private getters are omitted
            var properties = info.GetRuntimeProperties();            
            //var properties = info.GetProperties();
            int length = properties.Count();
            if (length == 0)
                return Array.Empty<PropertyModel>();
            var tempProps = new PropertyModel[length];
            length = 0; // reset length as index in loop below
            foreach (var property in properties)
                tempProps[length++] = new PropertyModel(property)
                {
                    Comments = reader.GetMemberComments(property)
                };
            return tempProps; 
        }

        /// <summary>
        /// Collection of members always present in an object.
        /// Works for structs too because they are <see cref="ValueType"/> which is a class behind the scenes.
        /// </summary>
        static readonly string[] DEFAULT_OBJ_METHODS = typeof(object).GetRuntimeMethods().Select(m => m.Name).ToArray();

        public MethodModel[] GetMethods(TypeInfo info, DocXmlReader reader)
        {
            // Filter out default methods and compiler generated
            var methods = info
                .GetRuntimeMethods()
                .Where(method => !method.IsSpecialName &&
                !DEFAULT_OBJ_METHODS.Any(name => name.Equals(method.Name))
            );

            int length = methods.Count();
            if (length == 0)
                return Array.Empty<MethodModel>();
            var tempMethods = new MethodModel[length];
            length = 0; // reset length as index in loop below
            foreach (var method in methods)
                tempMethods[length++] = new MethodModel(method)
                {
                    Comments = reader.GetMethodComments(method)
                };
            return tempMethods;
        }

        public EventModel[] GetEvents(TypeInfo info, DocXmlReader reader)
        {
            var events = info.GetRuntimeEvents();
            int length = events.Count();
            if (length == 0)
                return Array.Empty<EventModel>();
            var tempEvents = new EventModel[length];
            length = 0;
            foreach (var _event in events)
                tempEvents[length++] = new EventModel(_event)
                {
                    Comments = reader.GetMemberComments(_event)
                };
            return tempEvents;
        }
    }
}
