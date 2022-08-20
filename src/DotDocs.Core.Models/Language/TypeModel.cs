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

        public string? BaseType => Type.BaseType?.GetTypeId();

        public string? Namespace => Type.Namespace;

        public override string Name => Type.Name;

        public string? FullName => Type.FullName;

        public CommonComments Comments { get; set; }        

        #region Type Kind
        public bool IsClass => Type.IsClass;
        public bool IsInterface => Type.IsInterface;
        public bool IsValueType => Type.IsValueType;
        public bool IsEnum => Type.IsEnum;
        public bool IsDelegate => Type.BaseType?.FullName == "System.MulticastDelegate"; // Extra
        #endregion

        #region Members
        EventModel[] events;
        public EventModel[] Events
            => events ??= IsFacade ? Array.Empty<EventModel>() : Type
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
                    if (IsFacade)
                    {
                        fields = Array.Empty<FieldModel>();                       
                    }    
                    // When a type is an enum it's first property denotes the type of all members
                    else if (IsEnum)
                    {
                        var _fields = Type.GetRuntimeFields()
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
                        fields = Type
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
            => properties ??= IsFacade ? Array.Empty<PropertyModel>() : Type
                    .GetDesiredProperties()
                    .Select(property => new PropertyModel(property))
                    .ToArray();


        MethodModel[] methods;
        public MethodModel[] Methods
            => methods ??= IsFacade ? Array.Empty<MethodModel>() : Type
                    .GetDesiredMethods()
                    .Select(method => new MethodModel(method))
                    .ToArray();
        #endregion

        #region Generics
        /// <summary>
        /// Contains the primary keys to each type's definition.
        /// </summary>
        public string[] GenericTypeArguments => Type.GenericTypeArguments.Select(t => t.GetTypeId()).ToArray();
        /// <summary>
        /// Contains the primary keys to each type's definition.
        /// </summary>
        public string[] GenericTypeParameters => Type.GenericTypeParameters.Select(t => t.GetTypeId()).ToArray();
        /// <summary>
        /// Denotes if this type is constructed from a generic type.
        /// </summary>
        public bool IsConstructedGenericType => Type.IsConstructedGenericType;
        /// <summary>
        /// Denotes if this type is generic, meaning can be a generic type definition, open constructed type or closed constructed type.
        /// </summary>
        public bool IsGenericType => Type.IsGenericType;
        /// <summary>
        /// Denotes that this type defines a generic type and can be used to create constructed types.
        /// </summary>
        public bool IsGenericTypeDefinition => Type.IsGenericTypeDefinition;
        /// <summary>
        /// Indicates if this type is used as a generic parameter in a type definition or in a generic method definition.
        /// </summary>
        public bool IsGenericParameter => Type.IsGenericParameter;
        /// <summary>
        /// A token that is unique for each type, however constructed types and generic type definitions will share
        /// the same token.
        /// </summary>
        public int MetadataToken => Type.MetadataToken;
        #endregion

        string? typeId;
        public string Id
            => typeId ??= Type.GetTypeId();        

        public string AssemblyId => Type.Assembly.GetAssemblyId();

        [JsonIgnore]
        public TypeInfo Type { get; init; }

        [JsonIgnore]
        public bool IsFacade { get; init; }

        public TypeModel(Type type, bool isFacade)
        {
            Type = type.GetTypeInfo();
            IsFacade = isFacade;
        }

        public TypeModel(TypeInfo typeInfo, bool isFacade)
        {
            Type = typeInfo;
            IsFacade = isFacade;
        }
    }    
}
