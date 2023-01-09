using DotDocs.Core.Models.Language.Members;
using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DotDocs.Core.Models.Language
{
    /// <summary>
    /// Contains extra organized information for documenting a user type.
    /// </summary>
    public class UserTypeModel : TypeModel
    {                              
        #region Members
        EventModel[] events;
        public EventModel[] Events
            => events ??= Info
                    .GetRuntimeEvents()
                    .Select(_event => new EventModel(_event))
                    .ToArray();

        FieldModel[] fields;
        public FieldModel[] Fields
        {
            get
            {                
                if (fields == null)
                {
                    if (IsEnum)
                    {
                        var _fields = Info.GetRuntimeFields()
                            .Where(_field => !_field
                                .GetCustomAttributesData()
                                .Any(attr => attr.AttributeType.Name == typeof(CompilerGeneratedAttribute).Name))
                            .ToArray();
                        var type = _fields[0].FieldType;
                        var models = new FieldModel[_fields.Length - 1]; // Omit first field
                        for (int i = 1; i < _fields.Length; i++)                        
                            models[i - 1] = new FieldModel(_fields[i], type);                        
                        fields = models;
                    }
                    else
                    {
                        fields = Info
                            .GetRuntimeFields()
                            .Where(_field => !_field
                                .GetCustomAttributesData()
                                .Any(attr => attr.AttributeType.Name == typeof(CompilerGeneratedAttribute).Name) &&
                            !_field.Attributes.HasFlag(FieldAttributes.SpecialName) &&
                            !_field.Attributes.HasFlag(FieldAttributes.RTSpecialName))
                            .Select(_field => new FieldModel(_field))
                            .ToArray();
                    }                    
                }
                return fields;
            }
        }

        PropertyModel[] properties;
        public PropertyModel[] Properties
            => properties ??= Info
                    .GetDesiredProperties()
                    .Select(property => new PropertyModel(property))
                    .ToArray();


        MethodModel[] methods;
        public MethodModel[] Methods
            => methods ??= Info
                    .GetDesiredMethods()
                    .Select(method => new MethodModel(method))
                    .ToArray();
        #endregion

        public UserTypeModel(Type type) : base(type.GetTypeInfo()) { }
        public UserTypeModel(TypeInfo typeInfo) : base(typeInfo) { }

        public override void Add(Dictionary<Type, TypeModel> allModels, Dictionary<Assembly, AssemblyModel<TypeModel>> assemblies)
        {
            base.Add(allModels, assemblies);

            // Add properties
            foreach (var property in Properties)
                AddType(property.Info.PropertyType, allModels, assemblies);
            // Add fields
            foreach (var field in Fields)
                AddType(field.Info.FieldType, allModels, assemblies);
            // Add methods
            foreach (var method in Methods)
            {
                AddType(method.Info.ReturnType, allModels, assemblies);
                var parameters = method.Info.GetParameters();
                foreach (var parameter in parameters)
                    AddType(parameter.ParameterType, allModels, assemblies);
            }
            // Add events
            foreach (var _event in Events)
                if (_event.Info.EventHandlerType != null)
                    AddType(_event.Info.EventHandlerType, allModels, assemblies);

            // Ensure implemented interfaces are accounted for
            //foreach (var _interface in Info.GetDesiredInterfaces())
            //    AddType(_interface, allModels, assemblies);
        }

        /// <summary>
        /// Entry-point for rendering types to file stream.
        /// </summary>
        /// <param name="fileStream"></param>
        public void Document(Stream fileStream)
        {
            fileStream.Write(Encoding.UTF8.GetBytes("Hello World!"));
            // From here render all contents for type
        }

        public void Document(string basePath)
        {
            var folderPath = Path.Join(basePath, GetNamespaceAsPath());

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            using var writer = File.CreateText(Path.Join(folderPath, Info.Name) + ".md");
            
            writer.Write(Info.FullName);
        }

        string GetNamespaceAsPath()
            => string.Join("/", Info.Namespace.Split('.'));
    }    
}
