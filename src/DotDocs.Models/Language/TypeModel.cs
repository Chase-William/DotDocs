using DotDocs.Models.Language.Members;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models.Language
{
    public class TypeModel : ITypeable
    {
        public string AssemblyQualifiedName { get; set; }
        //public bool ContainsGenericParameters { get; set; }        
        
        public string Namespace { get; set; }
        public string Name { get; set; }
        // public string FullName { get; set; }

        //public int GenericParameterPosition { get; set; }
        //public bool HasElementType { get; set; }
        //public bool IsAbstract { get; set; }
        //public bool IsAnsiClass { get; set; }
        //public bool IsArray { get; set; }
        //public bool IsAutoClass { get; set; }
        //public bool IsAutoLayout { get; set; }
        //public bool IsByRef { get; set; }
        //public bool IsByRefLike { get; set; }
        //public bool IsClass { get; set; }
        //public bool IsCOMObject { get; set; }
        //public bool IsConstructedGenericType { get; set; }
        //public bool IsContextful { get; set; }
        //public bool IsEnum { get; set; }
        //public bool IsExplicitLayout { get; set; }
        //public bool IsGenericMethodParameter { get; set; }
        //public bool IsGenericParameter { get; set; }
        //public bool IsGenericType { get; set; }
        //public bool IsGenericTypeDefinition { get; set; }
        //public bool IsGenericTypeParameter { get; set; }
        //public bool IsInterface { get; set; }
        //public bool IsLayoutSequential { get; set; }
        //public bool IsMarshalByRef { get; set; }
        //public bool IsNested { get; set; }
        //public bool IsNestedAssembly { get; set; }
        //public bool IsNestedFamANDAssem { get; set; }
        //public bool IsNestedFamily { get; set; }
        //public bool IsNestedFamORAssem { get; set; }
        //public bool IsNestedPrivate { get; set; }
        //public bool IsNestedPublic { get; set; }
        //public bool IsPublic { get; set; }
        //public bool IsSealed { get; set; }
        //public bool IsSerializable { get; set; }
        //public bool IsSignatureType { get; set; }
        //public bool IsSZArray { get; set; }
        //public bool IsTypeDefinition { get; set; }
        //public bool IsUnicodeClass { get; set; }
        //public bool IsValueType { get; set; }
        //public bool IsVariableBoundArray { get; set; }
        //public bool IsVisible { get; set; }
        //public bool IsSpecialName { get; set; }

        public FieldModel[] Fields { get; set; }

        public MethodModel[] Methods { get; set; }

        public PropertyModel[] Properties { get; set; }

        public EventModel[] Events { get; set; }

        public ITypeable? BaseType { get; set; }

        const BindingFlags DEFAULT_SEARCH = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;

        public TypeModel(
            Type type,
            ImmutableDictionary<string, AssemblyModel> assemblies,
            Dictionary<string, ITypeable> types
            ) {

            // Add before processing to prevent members who use this type from calling the constructor again    
            var key = type.GetUniqueName();                    
            if (!types.ContainsKey(key))
                types.Add(key, this);
            // ^ Duplicate inside TypeModel, where can we move this to that it makes sense?

            Namespace = type.Namespace;
            Name = type.Name;
            // FullName = type.FullName;

            // Generic Type Parameters
            // ITypeable.TestingTypeParams(type, assemblies, types);
            if (type.IsGenericType)
            {
                Console.WriteLine();
            }
            if (type.BaseType.IsConstructedGenericType)
            {
                
                var a = type.GenericTypeArguments;
                var p = type.GetTypeInfo().GenericTypeParameters;
                var t = type.BaseType.GetTypeInfo().GenericTypeArguments[1].GetTypeInfo();
                Console.WriteLine();
            }               

            // Gather BaseType info for TypeModels only
            if (type.BaseType is not null)
                BaseType = ITypeable.GetOrCreateTypeFrom(type.BaseType, assemblies, types);

            // Fields
            // TODO: Needs Testing
            var fields = type.GetFields(DEFAULT_SEARCH);
            Fields = new FieldModel[fields.Length];
            int i = 0;
            for (; i < fields.Length; i++)
                Fields[i] = new FieldModel(fields[i], assemblies, types); //.Apply(fields[i]);

            // Methods
            // TODO: Needs Testing
            // Returns exported methods that are also not compiler generated i.e. property getter/setter
            var methods = type.GetMethods(DEFAULT_SEARCH)
                .Where(m => !m.Attributes.HasFlag(MethodAttributes.SpecialName))
                .ToArray();
            Methods = new MethodModel[methods.Length];
            for (i = 0; i < methods.Length; i++)
                Methods[i] = new MethodModel(methods[i], assemblies, types); // .Apply(methods[i]);

            // Properties
            // TODO: Needs Testing
            var properties = type.GetProperties(DEFAULT_SEARCH);
            Properties = new PropertyModel[properties.Length];
            for (i = 0; i < properties.Length; i++)
                Properties[i] = new PropertyModel(properties[i], assemblies, types);

            // Events
            // TODO: Needs Testing
            var events = type.GetEvents(DEFAULT_SEARCH);
            Events = new EventModel[events.Length];
            for (i = 0; i < events.Length; i++)
                Events[i] = new EventModel(events[i], assemblies, types);
        }
    }
}
