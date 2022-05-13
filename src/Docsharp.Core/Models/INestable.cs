using System;
using System.Linq;
using System.Reflection;

using Docsharp.Core.Models.Members;

namespace Docsharp.Core.Models
{
    /// <summary>
    /// Represents a type that can contain Properties, Fields, and Methods.
    /// </summary>
    public interface INestable
    {
        public PropertyModel[] Properties { get; set; }
        public FieldModel[] Fields { get; set; }
        public MethodModel[] Methods { get; set; }

        public static void Initialize(INestable constructable, TypeInfo info)
        {
            constructable.Properties = constructable.GetProperties(info);
            constructable.Fields = constructable.GetFields(info);
            constructable.Methods = constructable.GetMethods(info);
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

        public FieldModel[] GetFields(TypeInfo info)
        {
            var fields = info.GetFields();
            if (fields.Length == 0)
                return Array.Empty<FieldModel>();
            var tempFields = new FieldModel[fields.Length];
            for (int i = 0; i < fields.Length; i++)
                tempFields[i] = new FieldModel(fields[i]);
            return tempFields;
        }

        public MethodModel[] GetMethods(TypeInfo info)
        {
            var methods = info.DeclaredMethods;
            int length = methods.Count();
            if (length == 0)
                return Array.Empty<MethodModel>();
            var tempMethods = new MethodModel[length];
            length = 0; // reset length as index in loop below
            foreach (var method in methods)
                tempMethods[length++] = new MethodModel(method);
            return tempMethods;
        }
    }
}
