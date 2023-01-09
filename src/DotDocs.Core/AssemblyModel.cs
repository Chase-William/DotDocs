using DotDocs.Core.Build;
using DotDocs.Core.Language;
using DotDocs.Core.Util;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DotDocs.Core
{
    /// <summary>
    /// A class that represents an assembly as a model.
    /// </summary>
    public class AssemblyModel<TModel> where TModel : TypeModel
    {
        /// <summary>
        /// Id for MongoDb records.
        /// </summary>
        public ObjectId Id { get; init; }

        [BsonIgnore]
        /// <summary>
        /// The config contains the display properties for all types and their members. It determines what data should
        /// be kept and discarded. All assembly models that represent a user's assembly MUST have a configuration instance.
        /// Any assembly that is a supporting assembly (not produced by the user's project) should not have a config instance
        /// and will only present public information for referencing. This is to prevent private or un-used information about
        /// supporting libraries from being process.
        /// </summary>
        readonly Configuration? config;

        public string Name { get; set; }

        public Version Version { get; set; }

        [BsonIgnore]
        private Assembly assembly;
        [BsonIgnore]
        public Assembly Assembly
        {
            get => assembly;
            init
            {
                assembly = value;
                Name = Assembly.GetName().Name;
                Version = Assembly.GetName().Version;
            }
        }

        [BsonIgnore]
        /// <summary>
        /// A reference to the local project that creates this assembly if it exists in the context.
        /// </summary>        
        public ProjectBuildInstance? Build { get; init; }        

        [BsonIgnore]
        /// <summary>
        /// Contains all the desired types defined within this assembly.
        /// </summary>        
        public List<TModel> TypeModels { get; private set; } 

        public AssemblyModel() { }

        /// <summary>
        /// Creates a new instance of the <see cref="AssemblyModel"/> class for a user's assembly.
        /// </summary>
        /// <param name="build"></param>
        /// <param name="assembly"></param>
        /// <param name="config"></param>
        public AssemblyModel(ProjectBuildInstance build, Configuration config)
        {
            Build = build;
            Assembly = build.Assembly;
            this.config = config;
            TypeModels = GetModels()
                .Cast<TModel>()
                .ToList();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="AssemblyModel"/> class for supporting libraries.
        /// </summary>
        /// <param name="assembly"></param>
        public AssemblyModel(Assembly assembly)
        {
            Assembly = assembly;
            TypeModels = new List<TModel>();
        }

        /// <summary>
        /// Returns a boolean indicating if this assembly is created by the user's project.
        /// </summary>
        public bool IsUserProject() => Build != null;

        List<UserTypeModel> GetModels()
        {            
            return Assembly.DefinedTypes
                .Where(info =>
                {
                    // Do not keep compiler generated types
                    if (info.CustomAttributes.Any(attr => attr.AttributeType.Name == typeof(CompilerGeneratedAttribute).Name))
                        return false;

                    if (info.IsClass)
                    {
                        if (info.BaseType?.FullName == "System.MulticastDelegate")
                            return Keep(info, config.Type.Delegate);
                        return Keep(info, config.Type.Class);
                    }
                    else if (info.IsInterface)
                        return Keep(info, config.Type.Interface);
                    else if (info.IsValueType)
                        return Keep(info, config.Type.Struct);
                    else if (info.IsEnum)
                        return Keep(info, config.Type.Enum);
                    throw new Exception($"Type {info} did not align with any configuration.");
                })
                .Select(info =>
                {
                    if (IsUserProject())
                        return new UserTypeModel(info);
                    throw new Exception($"The function {nameof(GetModels)} should never be called for supporting assemblies.");
                    // return new TypeModel(info);
                })
                .ToList();
        }
        
        bool Keep(TypeInfo info, AccessibleConfig instanceConfig)
        {
            if ((bool)instanceConfig.ShowIfPublic && info.IsPublic || info.IsNestedPublic)            
                return CheckNestedAccessibility(info);            
            else if ((bool)instanceConfig.ShowIfInternalProtected && info.IsProtected() && info.IsInternal())
                return CheckNestedAccessibility(info);
            else if ((bool)instanceConfig.ShowIfPrivateProtected && info.IsProtected() && info.IsPrivate())
                return CheckNestedAccessibility(info);
            else if ((bool)instanceConfig.ShowIfProtected && info.IsProtected())
                return CheckNestedAccessibility(info);
            else if ((bool)instanceConfig.ShowIfInternal && info.IsInternal())
                return CheckNestedAccessibility(info);
            else if ((bool)instanceConfig.ShowIfPrivate && info.IsPrivate())
                return CheckNestedAccessibility(info);

            return false;

            // throw new Exception($"Invalid branch of if statement reached when determining if the type: {info}, should be taken for documentation.");
        }

        bool CheckNestedAccessibility(TypeInfo info)
        {
            if (!info.IsNested)
                return true;

            if (config.Perspective == Perspective.Internal)
                return true;

            var current = info.DeclaringType;
            do
            {
                // Declaring type is inaccessible outside this type, do not keep
                if (current.IsPrivate() || current.IsInternal())
                    return false;
                current = current.DeclaringType;
            }
            while (current != null);
            return true; // No declaring types found to be inaccessible from outside this assembly
        }
    }   
}
