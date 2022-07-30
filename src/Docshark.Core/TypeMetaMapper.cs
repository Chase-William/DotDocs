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

namespace Docshark.Core
{
    /// <summary>
    /// Maps each type used in the target project.
    /// </summary>
    public static class TypeMetaMapper
    {
        public const string TYPE_META_MAPPER_FILENAME = "types.json";

        static TypeMap map = new TypeMap();

        public static void SaveTypes(string outputPath)
        {
            Utility.CleanDirectory(outputPath);
            using StreamWriter writer = new(Path.Combine(outputPath, TYPE_META_MAPPER_FILENAME));
            writer.Write(JsonSerializer.Serialize(map.Types.Values));
        }

        public static void Add(DelegateModel model)
            => map.AddType(model.Meta);        

        /// <summary>
        /// Adds various types of an element that inherits and implements the contraints.
        /// </summary>
        /// <typeparam name="T">Type of model.</typeparam>
        /// <param name="model">Instance of model.</param>
        public static void Add<T>(T model) where T : TypeMember<TypeInfo, TypeComments>, IMemberContainable
        {
            map.AddType(model.Meta);
            AddMembers(model);
        }

        static void AddMembers(IMemberContainable model)
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

        public static void Add(EnumModel model)
        {
            map.AddType(model.Meta);
            AddFields(model.Fields);
        }

        static void AddFields(FieldModel[] fields)
        {
            foreach (var mem in fields)
                map.AddType(mem.Meta.FieldType);
        }              
    } 
}
