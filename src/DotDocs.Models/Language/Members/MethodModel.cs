using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models.Language.Members
{
    public class MethodModel : MemberModel
    {
        //public bool ContainsGenericParameters { get; set; }
        //public bool IsAbstract { get; set; }
        //public bool IsConstructedGenericMethod { get; set; }
        //public bool IsConstructor { get; set; }
        //public bool IsFinal { get; set; }
        //public bool IsGenericMethod { get; set; }
        //public bool IsGenericMethodDefinition { get; set; }
        //public bool IsHideBySig { get; set; }
        //public bool IsVirtual { get; set; }

        public ParamInfoModel[] Parameters { get; set; }

        public ITypeable ReturnType { get; set; }

        public MethodModel(
            MethodInfo info,
            ImmutableDictionary<string, AssemblyModel> assemblies,
            Dictionary<string, ITypeable> types
            ) {
            Name = info.Name;
           
            

            ReturnType = ITypeable.GetOrCreateTypeFrom(info.ReturnType, assemblies, types);
            // Get parameter models from System.Reflection.ParameterInfo col
            Parameters = info.GetParameters().Select(p => new ParamInfoModel(p, assemblies, types)).ToArray();
            
        }
    }
}
