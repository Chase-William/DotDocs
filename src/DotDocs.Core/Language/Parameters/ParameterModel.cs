using System.Text.Json.Serialization;

namespace DotDocs.Core.Language.Parameters
{
    public class ParameterModel
    {        
        [JsonIgnore]
        public System.Reflection.ParameterInfo Info { get; init; }

        public ParameterModel(System.Reflection.ParameterInfo info)
        {
            Info = info;
        }
    }
}
