using Docshark.Core.Global.Assemblies;
using Docshark.Core.Global.Projects;
using Docshark.Core.Global.Types;
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
        public AssemblyMapper AssemblyMap { get; init; }
        public ProjectMapper ProjectMap { get; init; }

        public MapperManager()
        {
            AssemblyMap = new();
            TypeMap = new(AssemblyMap);
            ProjectMap = new(AssemblyMap);
        }

        public void Save(string baseOutputPath)
        {
            ((IMapper<TypeDefinition>)TypeMap).Save(
                baseOutputPath, 
                TypeMapper.TYPE_MAPPER_FILENAME, 
                TypeMap.MappedDefinitions.Values);         
            ((IMapper<AssemblyDefinition>)AssemblyMap).Save(
                baseOutputPath, 
                AssemblyMapper.ASSEMBLY_MAPPER_FILENAME, 
                AssemblyMap.MappedDefinitions.Values);
            ((IMapper<ProjectDefinition>)ProjectMap).Save(
                baseOutputPath,
                ProjectMapper.PROJECT_MAPPER_FILENAME,
                ProjectMap.MappedDefinitions.Values);
        }
    }
}
