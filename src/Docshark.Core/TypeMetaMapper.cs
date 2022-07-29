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

namespace Docshark.Core
{
    /// <summary>
    /// Maps each type used in the target project.
    /// </summary>
    public static class TypeMetaMapper
    {
        public const string TYPE_META_MAPPER_FILENAME = "types.json";

        static Dictionary<string, TypeDefinition> types = new();

        public static void SaveTypes(string outputPath)
        {
            Utility.CleanDirectory(outputPath);
            using StreamWriter writer = new(Path.Combine(outputPath, TYPE_META_MAPPER_FILENAME));
            writer.Write(JsonSerializer.Serialize(types.Values));
        }

        public static void Add(DelegateModel model)
            => AddSelfAndParent(model.Meta);        

        /// <summary>
        /// Adds various types of an element that inherits and implements the contraints.
        /// </summary>
        /// <typeparam name="T">Type of model.</typeparam>
        /// <param name="model">Instance of model.</param>
        public static void Add<T>(T model) where T : TypeMember<TypeInfo, TypeComments>, IMemberContainable
        {
            AddSelfAndParent(model.Meta);
            AddMembers(model);
        }

        static void AddMembers(IMemberContainable model)
        {
            foreach (var mem in model.Properties)            
                AddSelfAndParent(mem.Meta.PropertyType);
            AddFields(model.Fields);
            foreach (var mem in model.Events)
                AddSelfAndParent(mem.Meta.EventHandlerType);
            foreach (var mem in model.Methods)
            {
                AddSelfAndParent(mem.Meta.ReturnType);
                var parameters = mem.Meta.GetParameters();
                foreach (var param in parameters)                
                    AddSelfAndParent(param.ParameterType);                
            }
        }

        public static void Add(EnumModel model)
        {
            AddSelfAndParent(model.Meta);
            AddFields(model.Fields);
        }

        static void AddFields(FieldModel[] fields)
        {
            foreach (var mem in fields)
                AddSelfAndParent(mem.Meta.FieldType);
        }

        static void AddSelfAndParent(Type info)
        {
            if (!AddType(info))
                return;

            if (info.BaseType != null)
                AddType(info.BaseType);
        }
        
        static bool AddType(Type info)
        {
            // Can return because this is the root call of this method and this type is already added
            if (types.ContainsKey(info.ToString()))
                return false;

            if (info.FullName.Contains("Action"))
            {
                Console.WriteLine();
            }

            //var def = TypeDefinition.From(info);
            //types.Add(info.ToString(), def);            
            AddTypeRecursive(info);
            return true;
        }

        static TypeReference AddTypeRecursive(Type info, TypeReference reference = null, bool updateRef = false)
        {           
            if (info == null)
                return null;
            // Add type definition to dictionary if needed
            if (!types.ContainsKey(info.ToString()))
            {
                TypeDefinition def;
                def = TypeDefinition.From(info);
                if (reference == null)
                    reference = def;              
                types.Add(info.ToString(), def);
            }            

            // Map out the parent or base types if available
            if (info.BaseType != null)
            {
                // Update parent with reference
                reference.Parent = new TypeReference { TypeId = info.BaseType.ToString() };
                AddTypeRecursive(info.BaseType, reference.Parent);
            }                           

            var parameters = info.GenericTypeArguments;
            if (parameters.Length > 0)
                AddTypeArgumentsRecursive(parameters, updateRef ? types[info.ToString()] : reference);

            return reference;
        }

        // fix this, Type arguments are not being handled correctly
        static void AddTypeArgumentsRecursive(Type[] parameters, TypeReference reference)
        {            
            foreach (var param in parameters)
            {               
                string typeId = AddTypeRecursive(param, reference, param.GenericTypeArguments.Length > 0 ? true : false).TypeId;
                types[typeId].TypeArguments.Add(types[param.ToString()].TypeId);
            }        
        }
    }

    public class TypeDefinition : TypeReference
    {
        public List<string> TypeArguments { get; set; } = new();
        public string Namespace { get; set; }
        public string TypeName { get; set; }        
        public string AssemblyName { get; set; }
        public CommonComments Comments { get; set; }

        public static TypeDefinition From(Type info)
            => new()
            {
                TypeId = info.ToString(),
                Namespace = info.Namespace,
                TypeName = info.Name,
                AssemblyName = info.Assembly.FullName
            };
    }

    public class TypeReference
    {
        public string TypeId { get; set; }
        public TypeReference Parent { get; set; }
    }  
}
