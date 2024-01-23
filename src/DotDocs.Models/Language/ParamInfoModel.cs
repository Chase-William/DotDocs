using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models.Language
{
    public class ParamInfoModel
    {
        public ITypeable ParamType { get; set; }
        public string Name { get; set; }

        public ParamInfoModel(
            ParameterInfo info,
            ImmutableDictionary<string, AssemblyModel> assemblies,
            Dictionary<string, ITypeable> types
            ) {
            Name = info.Name;

            ParamType = ITypeable.GetOrCreateTypeFrom(info.ParameterType, assemblies, types);
        }
    }
}
