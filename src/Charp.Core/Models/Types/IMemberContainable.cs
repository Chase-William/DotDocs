using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Charp.Core.Models.Members;

using LoxSmoke.DocXml;

namespace Charp.Core.Models.Types
{
    /// <summary>
    /// Represents a entity that can contain properties, fields, events, and methods.
    /// </summary>
    public interface IMemberContainable : IFieldable
    {
        /// <summary>
        /// Collection of members always present in an object.
        /// Works for structs too because they are <see cref="ValueType"/> which is a class behind the scenes.
        /// </summary>
        static readonly string[] DEFAULT_OBJ_METHODS = typeof(object).GetRuntimeMethods().Select(m => m.Name).ToArray();

        /// <summary>
        /// Properties of this <see cref="IMemberContainable"/>.
        /// </summary>
        public PropertyModel[] Properties { get; set; }
        /// <summary>
        /// Methods of this <see cref="Methods"/>.
        /// </summary>
        public MethodModel[] Methods { get; set; }
        /// <summary>
        /// Events of this <see cref="IMemberContainable"/>.
        /// </summary>
        public EventModel[] Events { get; set; }      

        /// <summary>
        /// Loads <see cref="IMemberContainable"/>'s properties, fields, events, and methods with members.
        /// </summary>
        /// <param name="constructable">Instance of <see cref="IMemberContainable"/> to call methods on.</param>
        /// <param name="info">Data source to pull members from.</param>
        /// <param name="reader">Reader to acquire written documentation from.</param>
        public static void Init(IMemberContainable constructable, TypeInfo info, DocXmlReader reader)
        {
            constructable.Properties = constructable.GetProperties(info, reader);
            constructable.Fields = constructable.GetFields(info, reader);
            constructable.Methods = constructable.GetMethods(info, reader);
            constructable.Events = constructable.GetEvents(info, reader);
        }

        /// <summary>
        /// Gets all the desired properties from the type info with documentation.
        /// </summary>
        /// <param name="info">Information about the type.</param>
        /// <param name="reader">Used to get the written documentation.</param>
        /// <returns>Collection of desired properties.</returns>
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
        /// Gets all the desired methods from the type info with documentation.
        /// </summary>
        /// <param name="info">Information about the type.</param>
        /// <param name="reader">Used to get the written documentation.</param>
        /// <returns>Collection of desired methods.</returns>
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

        /// <summary>
        /// Gets all the desired events from the type info with documentation.
        /// </summary>
        /// <param name="info">Information about the type.</param>
        /// <param name="reader">Used to get the written documentation.</param>
        /// <returns>Collection of desired events.</returns>
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
