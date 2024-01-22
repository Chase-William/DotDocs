using DotDocs.Models.Language.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models.Language
{
    public class TypeModel : Model
    {
        public string AssemblyQualifiedName { get; set; }
        //public bool ContainsGenericParameters { get; set; }
        public string FullName { get; set; }
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

        public List<FieldModel> Fields { get; set; } = new();

        public string Name { get; set; }
        public string Namespace { get; set; }
    }
}
