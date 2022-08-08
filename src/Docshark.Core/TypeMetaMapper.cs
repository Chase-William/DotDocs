using Docshark.Core.Models.Codebase.Members;
using Docshark.Core.Models.Codebase.Types;
using LoxSmoke.DocXml;
using System.IO;
using System.Reflection;
using System.Text.Json;
using Docshark.Core.Models.Codebase;
using Docshark.Core.Global.Types;

namespace Docshark.Core
{
    /// <summary>
    /// Maps each type used in the target project.
    /// </summary>
    public class TypeMetaMapper
    {
        public const string TYPE_META_MAPPER_FILENAME = "types.json";

        TypeMapper map = new TypeMapper();

        public TypeMetaMapper() { }

        public void SaveTypes(string outputPath)
        {            
            using StreamWriter writer = new(Path.Combine(outputPath, TYPE_META_MAPPER_FILENAME));
            writer.Write(JsonSerializer.Serialize(map.Types.Values));
        }

        /// <summary>
        /// Adds given type and all dependent types to global type mapper if needed. 
        /// IMPORTANT: This method will perform a deep analysis of all types used in any manner by this type. 
        /// For example, types used in inheritance, encapsulated members, and even type arguments are analysed
        /// and added to the type mapper if needed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        public void AddType<T>(T type) where T : TypeMember<TypeInfo, TypeComments>
        {
            if (type is IMemberContainable containable) // Class, Struct, Interface
            {                
                map.AddType(type.Meta);
                AddMembers(containable);
                return;
            }
            else if (type is IFieldable fieldable) // Enum
            {                
                map.AddType(type.Meta);
                AddFields(fieldable.Fields);
                return;
            }
            // Delegate
            map.AddType(type.Meta);
        }

        void AddMembers(IMemberContainable model)
        {
            foreach (var mem in model.Properties)
                map.AddType(mem.Meta.PropertyType);
            AddFields(model.Fields);
            foreach (var mem in model.Events)
                map.AddType(mem.Meta.EventHandlerType);
            foreach (var mem in model.Methods)
            {
                map.AddType(mem.Meta.ReturnType);
                if (mem.MethodType != null)
                    map.AddType(mem.MethodType);
                var parameters = mem.Meta.GetParameters();
                foreach (var param in parameters)
                    map.AddType(param.ParameterType);                
            }
        }

        public void Add(EnumModel model)
        {
            map.AddType(model.Meta);
            AddFields(model.Fields);
        }

        void AddFields(FieldModel[] fields)
        {
            foreach (var mem in fields)
                map.AddType(mem.Meta.FieldType);
        }              
    } 
}
