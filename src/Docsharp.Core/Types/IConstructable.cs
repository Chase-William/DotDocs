using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace Docsharp.Core.Types
{
    public interface IConstructable
    {
        public PropertyMember[] Properties { get; set; }
        public FieldMember[] Fields { get; set; }
        public MethodMember[] Methods { get; set; }

        public static void Initialize(IConstructable constructable, TypeInfo info)
        {
            constructable.Properties = constructable.GetProperties(info);
            constructable.Fields = constructable.GetFields(info);
            constructable.Methods = constructable.GetMethods(info);
        }

        public PropertyMember[] GetProperties(TypeInfo info)
        {
            var props = info.GetProperties();
            if (props.Length == 0)
                return Array.Empty<PropertyMember>();
            var tempProps = new PropertyMember[props.Length];
            for (int i = 0; i < props.Length; i++)            
                tempProps[i] = new PropertyMember(props[i]);
            return tempProps;
        }

        public FieldMember[] GetFields(TypeInfo info)
        {
            var fields = info.GetFields();
            if (fields.Length == 0)
                return Array.Empty<FieldMember>();
            var tempFields = new FieldMember[fields.Length];
            for (int i = 0; i < fields.Length; i++)
                tempFields[i] = new FieldMember(fields[i]);
            return tempFields;
        }

        public MethodMember[] GetMethods(TypeInfo info)
        {
            var methods = info.GetMethods();
            if (methods.Length == 0)
                return Array.Empty<MethodMember>();
            var tempMethods = new MethodMember[methods.Length];
            for (int i = 0; i < methods.Length; i++)
                tempMethods[i] = new MethodMember(methods[i]);
            return tempMethods;
        }
    }
}
