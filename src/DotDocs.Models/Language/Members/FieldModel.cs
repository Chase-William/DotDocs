using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models.Language.Members
{
    public class FieldModel : MemberModel
    {        
        //public bool IsInitOnly { get; set; }
        //public bool IsLiteral { get; set; }
        //public bool IsNotSerialized { get; set; }
        //public bool IsPInvokeImpl { get; set; }      
        //public bool IsSecurityCritical { get; set; }
        //public bool IsSecuritySafeCritical { get; set; }
        //public bool IsSecurityTransparent { get; set; }        
        public string Name { get; set; }

        public ITypeable FieldType { get; set; }

        public FieldModel(
            FieldInfo info,
            ImmutableDictionary<string, AssemblyModel> assemblies,
            Dictionary<string, ITypeable> types
            ) {
            Name = info.Name;

            FieldType = ITypeable.GetOrCreateTypeFrom(info.FieldType, assemblies, types);
        }        
    }
}
