using Docshark.Core.Global.Assemblies;
using Docshark.Core.Global.Projects;
using Docshark.Core.Global.Types;
using Docshark.Core.Global.Types.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Docshark.Core.Global
{
    public class MapperManager
    {                        
        public TypeMapper TypeMap { get; init; }
        public GenericTypeMapper GenericTypeMapper { get; init; }
        public AssemblyMapper AssemblyMap { get; init; }
        public ProjectMapper ProjectMap { get; init; }

        /// <summary>
        /// A map of the primary keys for each type.
        /// </summary>
        PrimaryKeyMap[] PrimaryKeyMap = new PrimaryKeyMap[3];
        public MapperManager()
        {
            AssemblyMap = new();
            GenericTypeMapper = new();
            TypeMap = new(GenericTypeMapper, AssemblyMap);
            ProjectMap = new(AssemblyMap);
            PrimaryKeyMap[0] = new PrimaryKeyMap
            {
                DefinitionTypeName = nameof(TypeDefinition),
                PrimaryKeyName = TypeDefinition.GetPrimaryKeyMemberName()
            };
            PrimaryKeyMap[1] = new PrimaryKeyMap
            {
                DefinitionTypeName = nameof(AssemblyDefinition),
                PrimaryKeyName = AssemblyDefinition.GetPrimaryKeyMemberName()
            };
            PrimaryKeyMap[2] = new PrimaryKeyMap
            {
                DefinitionTypeName = nameof(ProjectDefinition),
                PrimaryKeyName = ProjectDefinition.GetPrimaryKeyMemberName()
            };
        }

        public void Save(string baseOutputPath)
        {
            ((IMapper<TypeDefinition>)TypeMap).Save(
                baseOutputPath, 
                TypeMapper.TYPE_MAPPER_FILENAME, 
                TypeMap.MappedDefinitions.Values);
            ((IMapper<GenericTypeDefinition>)GenericTypeMapper).Save(
                baseOutputPath,
                GenericTypeMapper.GENERIC_TYPE_MAPPER_FILENAME,
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
