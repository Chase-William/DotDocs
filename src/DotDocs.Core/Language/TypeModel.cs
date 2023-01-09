using DotDocs.Core.Comments;
using System.Reflection;

namespace DotDocs.Core.Language
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
        /// A reference to the actual <see cref="TypeConfig"/> instance for this <see cref="UserTypeModel"/>.
        /// </summary>           
        public TypeInfo Info { get; init; }

        /// <summary>
        /// Contains the developer documentation associated with this type if it is provided.
        /// </summary>
        public TypeCommentsModel? Comments { get; set; }

        public override string Name => Info.Name;

        public TypeModel(Type type)
        {
            Info = type.GetTypeInfo();
        }

        public TypeModel(TypeInfo typeInfo)
        {
            Info = typeInfo;            
        }

        public virtual void Add(Dictionary<Type, TypeModel> allModels, Dictionary<Assembly, AssemblyModel<TypeModel>> assemblies)
        {
            // Do not add if already accounted for
            //if (allModels.ContainsKey(Info.GetTypeId()))
            //    return;

            // Ensure the base type is added too
            if (Info.BaseType != null)
                AddType(Info.BaseType, allModels, assemblies);

            // Ensure all generic parameters are accounted for
            if (Info.ContainsGenericParameters) // Process generic parameters            
                foreach (Type param in Info.GenericTypeParameters)
                    AddType(param, allModels, assemblies);

            // Ensure all type argument types are accounted for
            if (Info.GenericTypeArguments.Length > 0)
                foreach (Type arg in Info.GenericTypeArguments)
                    if (!allModels.ContainsKey(arg))
                        AddType(arg, allModels, assemblies);

            // Ensure the element type for a arrays are accounted for
            if (Info.IsArray && Info.HasElementType)            
                AddType(
                    Info.GetElementType() ?? throw new Exception($"The element type of {Info.FullName} was null."), 
                    allModels, 
                    assemblies);
        }

        protected static void AddType(Type type, Dictionary<Type, TypeModel> allModels, Dictionary<Assembly, AssemblyModel<TypeModel>> assemblies)
        {
            // Do not add if already accounted for
            if (allModels.ContainsKey(type))
                return;

            var model = new TypeModel(type);
            allModels.Add(model.Info, model);

            AssemblyModel<TypeModel> asmModel;
            if (!assemblies.ContainsKey(type.Assembly))
            {
                asmModel = new AssemblyModel<TypeModel>(type.Assembly);
                assemblies.Add(asmModel.Assembly, asmModel);
            }
            else
                asmModel = assemblies[type.Assembly];
            // Add this used supporting type to the assembly model
            asmModel.TypeModels.Add(model);

            // Check the model recursively
            model.Add(allModels, assemblies);
        }
    }
}
