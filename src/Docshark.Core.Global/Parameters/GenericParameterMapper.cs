using Docshark.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Core.Global.Parameters
{
    public class GenericParameterMapper : IMapper<GenericParameterDefinition>
    {
        public const string GENERIC_TYPE_MAPPER_FILENAME = "parameters.json";

        public IReadOnlyDictionary<string, GenericParameterDefinition> MappedDefinitions => mappedGenericDefinitions;

        Dictionary<string, GenericParameterDefinition> mappedGenericDefinitions = new();

        public void Add(Type info)
        {
            mappedGenericDefinitions.Add(info.GetPrimaryKey(), new GenericParameterDefinition
            {
                BaseType = info.BaseType?.GetPrimaryKey(),
                Name = info.Name,
                NameWithBase = info.GetPrimaryKey()
            });
        }
    }
}
