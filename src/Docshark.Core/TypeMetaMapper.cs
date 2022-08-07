using Docshark.Core.Models.Lang.Members;
using Docshark.Core.Models.Lang.Types;
using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Docshark.Core.TypeMapper;
using Docshark.Core.Models.Lang;

namespace Docshark.Core
{
    /// <summary>
    /// Maps each type used in the target project.
    /// </summary>
    public class TypeMetaMapper
    {
        public const string TYPE_META_MAPPER_FILENAME = "types.json";

        TypeMap map = new TypeMap();

        public TypeMetaMapper() { }

        public void SaveTypes(string outputPath)
        {
            Utility.CleanDirectory(outputPath);
            using StreamWriter writer = new(Path.Combine(outputPath, TYPE_META_MAPPER_FILENAME));
            writer.Write(JsonSerializer.Serialize(map.Types.Values));
        }

        public void Add(DelegateModel model)
            => map.AddType(model.Meta);        

        public void AddTypeMember<T>(T type) where T : TypeMember<TypeInfo, TypeComments>
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

        /// <summary>
        /// Adds various types of an element that inherits and implements the contraints.
        /// </summary>
        /// <typeparam name="T">Type of model.</typeparam>
        /// <param name="model">Instance of model.</param>
        public void Add<T>(T model) where T : TypeMember<TypeInfo, TypeComments>, IMemberContainable
        {
            map.AddType(model.Meta);
            AddMembers(model);
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
