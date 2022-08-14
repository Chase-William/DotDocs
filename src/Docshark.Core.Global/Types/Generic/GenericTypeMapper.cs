using Docshark.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Core.Global.Types.Generic
{
    public class GenericTypeMapper : IMapper<GenericTypeDefinition>
    {
        public const string GENERIC_TYPE_MAPPER_FILENAME = "generic-types.json";

        public IReadOnlyDictionary<string, GenericTypeDefinition> MappedDefinitions => mappedGenericDefinitions;

        Dictionary<string, GenericTypeDefinition> mappedGenericDefinitions = new(); 
        
        public void Add(Type info)
        {
            mappedGenericDefinitions.Add(info.GetPrimaryKey(), new GenericTypeDefinition
            {
                BaseType = info.BaseType?.GetPrimaryKey(),
                Name = info.Name,
                NameWithBase = info.GetPrimaryKey()
            });
        }
    }
}
