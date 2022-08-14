using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Docshark.Core.Global
{
    internal interface IMapper<T> 
        where T : Definition
    {
        IReadOnlyDictionary<string, T> MappedDefinitions { get; }

        public void Save(string baseOutputPath, string fileName, IEnumerable<T> values)
        {
            using StreamWriter writer = new(Path.Combine(baseOutputPath, fileName));
            writer.Write(JsonSerializer.Serialize(values));
        }
    }
}
