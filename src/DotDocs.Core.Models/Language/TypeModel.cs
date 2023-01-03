using DotDocs.Core.Models.Mongo.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Core.Models.Language
{
    /// <summary>
    /// Contains the core information for a given type.
    /// </summary>
    public class TypeModel : Model
    {
        #region Type Kind
        public bool IsClass => Info.IsClass;
        public bool IsInterface => Info.IsInterface;
        public bool IsValueType => Info.IsValueType;
        public bool IsEnum => Info.IsEnum;
        public bool IsDelegate => Info.BaseType?.FullName == "System.MulticastDelegate"; // Extra
        #endregion

        /// <summary>
        /// A reference to the actual <see cref="Type"/> instance for this <see cref="UserTypeModel"/>.
        /// </summary>           
        public TypeInfo Info { get; init; }

        /// <summary>
        /// Contains the developer documentation associated with this type if it is provided.
        /// </summary>
        public TypeCommentsModel? Comments { get; set; }

        public override string Name => throw new NotImplementedException();

        public TypeModel(Type type)
        {
            Info = type.GetTypeInfo();
        }

        public TypeModel(TypeInfo typeInfo)
        {
            Info = typeInfo;            
        }

        public virtual void Add(Dictionary<string, TypeModel> allModels)
        {
            // Do not add if already accounted for
            if (allModels.ContainsKey(Info.GetTypeId()))
                return;

            // Ensure all generic parameters are accounted for
            if (Info.ContainsGenericParameters) // Process generic parameters            
                foreach (Type param in Info.GenericTypeParameters)
                    AddType(param, allModels);

            // Ensure all type argument types are accounted for
            if (Info.GenericTypeArguments.Length > 0)
                foreach (Type arg in Info.GenericTypeArguments)
                    if (!allModels.ContainsKey(arg.GetTypeId()))
                        AddType(arg, allModels);

            // Ensure the element type for a arrays are accounted for
            if (Info.IsArray && Info.HasElementType)            
                AddType(
                    Info.GetElementType() ?? throw new Exception($"The element type of {Info.FullName} was null."), 
                    allModels);
        }

        protected void AddType(Type type, Dictionary<string, TypeModel> allModels)
        {
            // Do not add if already accounted for
            if (allModels.ContainsKey(Info.GetTypeId()))
                return;

            var model = new TypeModel(type);
            allModels.Add(model.Info.GetTypeId(), model);
            model.Add(allModels);
        }
    }
}
