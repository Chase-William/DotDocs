using DotDocs.Core.Models.Exceptions;
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
    public class TypeModel : Model
    {
        [JsonIgnore]
        /// <summary>
        /// Collection of members always present in an object.
        /// Works for structs too because they are <see cref="ValueType"/> which is a class behind the scenes.
        /// </summary>
        static readonly string[] DEFAULT_OBJECT_METHODS = typeof(object).GetRuntimeMethods().Select(m => m.Name).ToArray();

        public string? BaseType => Info.BaseType?.GetTypeId();

        public string? Namespace => Info.Namespace;

        public override string Name => Info.Name;

        public string? FullName => Info.FullName;

        public TypeComments? Comments { get; set; }        

        #region Type Kind
        public bool IsClass => Info.IsClass;
        public bool IsInterface => Info.IsInterface;
        public bool IsValueType => Info.IsValueType;
        public bool IsEnum => Info.IsEnum;
        public bool IsDelegate => Info.BaseType?.FullName == "System.MulticastDelegate"; // Extra
        #endregion

        #region Members
        EventModel[] events;
        public EventModel[] Events
            => events ??= TreatAsFacade ? Array.Empty<EventModel>() : Info
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
                    if (TreatAsFacade)
                    {
                        fields = Array.Empty<FieldModel>();                       
                    }    
                    // When a type is an enum it's first property denotes the type of all members
                    else if (IsEnum)
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
            => properties ??= TreatAsFacade ? Array.Empty<PropertyModel>() : Info
                    .GetDesiredProperties()
                    .Select(property => new PropertyModel(property))
                    .ToArray();


        MethodModel[] methods;
        public MethodModel[] Methods
            => methods ??= TreatAsFacade ? Array.Empty<MethodModel>() : Info
                    .GetDesiredMethods()
                    .Select(method => new MethodModel(method))
                    .ToArray();
        #endregion

        #region Generics
        /// <summary>
        /// Contains the primary keys to each type's definition.
        /// </summary>
        public string[] GenericTypeArguments => Info.GenericTypeArguments.Select(t => t.GetTypeId()).ToArray();
        /// <summary>
        /// Contains the primary keys to each type's definition.
        /// </summary>
        public string[] GenericTypeParameters => Info.GenericTypeParameters.Select(t => t.GetTypeId()).ToArray();
        /// <summary>
        /// Denotes if this type is constructed from a generic type.
        /// </summary>
        public bool IsConstructedGenericType => Info.IsConstructedGenericType;
        /// <summary>
        /// Denotes if this type is generic, meaning can be a generic type definition, open constructed type or closed constructed type.
        /// </summary>
        public bool IsGenericType => Info.IsGenericType;
        /// <summary>
        /// Denotes that this type defines a generic type and can be used to create constructed types.
        /// </summary>
        public bool IsGenericTypeDefinition => Info.IsGenericTypeDefinition;
        /// <summary>
        /// Indicates if this type is used as a generic parameter in a type definition or in a generic method definition.
        /// </summary>
        public bool IsGenericParameter => Info.IsGenericParameter;
        
        public int MetadataToken => Info.MetadataToken;
        #endregion

        /// <summary>
        /// Denotes if this type is actually an array type.
        /// </summary>
        public bool IsArray => Info.IsArray;
        /// <summary>
        /// Denotes if this type is a by ref type.
        /// </summary>
        public bool IsByRef => Info.IsByRef;

        string? typeId;
        public string Id
            => typeId ??= Info.GetTypeId();        

        public string AssemblyId => Info.Assembly.GetAssemblyId();

        [JsonIgnore]
        public TypeInfo Info { get; init; }

        /// <summary>
        /// Denotes whether this type should be treated as a facade. Facade in this context means
        /// to avoid retaining extra information and only keeping essentials like the type definition itself.
        /// </summary>
        [JsonIgnore]
        public bool TreatAsFacade => IsArray || IsByRef || !IsDefinedInLocalProject || IsGenericParameter;

        /// <summary>
        /// Denotes whether this type was defined inside a local project.
        /// </summary>
        [JsonIgnore]        
        public bool IsDefinedInLocalProject { get; init; }

        [JsonIgnore]
        public AssemblyModel Assembly { get; set; }

        public TypeModel(Type type, bool isDefinedInLocalProject)
        {
            Info = type.GetTypeInfo();
            IsDefinedInLocalProject = isDefinedInLocalProject;
        }

        public TypeModel(TypeInfo typeInfo, bool isDefinedInLocalProject)
        {
            Info = typeInfo;
            IsDefinedInLocalProject = isDefinedInLocalProject;
        }
    }    
}
