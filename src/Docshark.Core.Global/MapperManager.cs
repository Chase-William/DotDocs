using Docshark.Core.Global.Assemblies;
using Docshark.Core.Global.Projects;
using Docshark.Core.Global.Types;
using Docshark.Core.Global.Parameters;
using System.Text.Json;

namespace Docshark.Core.Global
{
    public class MapperManager
    {                        
        public TypeMapper TypeMap { get; init; }
        public GenericParameterMapper GenericTypeMapper { get; init; }
        public AssemblyMapper AssemblyMap { get; init; }
        public ProjectMapper ProjectMap { get; init; }

        /// <summary>
        /// A map of the primary keys for each type.
        /// </summary>
        PrimaryKeyMap[] PrimaryKeyMap = new PrimaryKeyMap[4];
        public MapperManager()
        {
            AssemblyMap = new();
            GenericTypeMapper = new();
            TypeMap = new(GenericTypeMapper, AssemblyMap);
            ProjectMap = new(AssemblyMap);
            PrimaryKeyMap[0] = new PrimaryKeyMap
            {
                DefinitionTypeName = nameof(TypeDefinition),
                PrimaryKeyMemberName = TypeDefinition.GetPrimaryKeyMemberName()
            };
            PrimaryKeyMap[1] = new PrimaryKeyMap
            {
                DefinitionTypeName = nameof(AssemblyDefinition),
                PrimaryKeyMemberName = AssemblyDefinition.GetPrimaryKeyMemberName()
            };
            PrimaryKeyMap[2] = new PrimaryKeyMap
            {
                DefinitionTypeName = nameof(ProjectDefinition),
                PrimaryKeyMemberName = ProjectDefinition.GetPrimaryKeyMemberName()
            };
            PrimaryKeyMap[3] = new PrimaryKeyMap
            {
                DefinitionTypeName = nameof(GenericParameterDefinition),
                PrimaryKeyMemberName = GenericParameterDefinition.GetPrimaryKeyMemberName(),
                IsComposite = true
            };
        }

        public void Save(string baseOutputPath)
        {
            ((IMapper<TypeDefinition>)TypeMap).Save(
                baseOutputPath, 
                TypeMapper.TYPE_MAPPER_FILENAME, 
                TypeMap.MappedDefinitions.Values);
            ((IMapper<GenericParameterDefinition>)GenericTypeMapper).Save(
                baseOutputPath,
                GenericParameterMapper.GENERIC_TYPE_MAPPER_FILENAME,
                GenericTypeMapper.MappedDefinitions.Values);
            ((IMapper<AssemblyDefinition>)AssemblyMap).Save(
                baseOutputPath, 
                AssemblyMapper.ASSEMBLY_MAPPER_FILENAME, 
                AssemblyMap.MappedDefinitions.Values);
            ((IMapper<ProjectDefinition>)ProjectMap).Save(
                baseOutputPath,
                ProjectMapper.PROJECT_MAPPER_FILENAME,
                ProjectMap.MappedDefinitions.Values);
            
            // Save map for primary keys
            using var writer = new StreamWriter(Path.Combine(baseOutputPath, "_keys.json"));
            writer.Write(JsonSerializer.Serialize(PrimaryKeyMap));
        }
    }
}
