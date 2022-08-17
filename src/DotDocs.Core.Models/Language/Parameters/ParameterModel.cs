using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DotDocs.Core.Models.Language.Parameters
{
    public class ParameterModel
    {
        public string? Name => Info.Name;

        public string Type => Info.ParameterType.GetTypeId();

        [JsonIgnore]
        public System.Reflection.ParameterInfo Info { get; init; }

        public ParameterModel(System.Reflection.ParameterInfo info)
        {
            Info = info;
        }
    }
}
