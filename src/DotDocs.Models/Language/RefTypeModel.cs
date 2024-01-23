using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models.Language
{    
    public interface IContextable
    {

    }

    public class Default
    {

    }

    public class AsParameter
    {

    }

    public class RefTypeModel : ITypeable
    {
        public string Namespace { get; set; }
        public string Name { get; set; }
        // public string FullName { get; set; }      



        public RefTypeModel(
            Type type, 
            ImmutableDictionary<string, AssemblyModel> assemblies, 
            Dictionary<string, ITypeable> types
            ) {
            Namespace = type.Namespace;
            Name = type.Name;
            // FullName = type.FullName;

            

            // Add before processing to prevent members who use this type from calling the constructor again            
            var key = type.GetUniqueName();
            if (!types.ContainsKey(key))
                types.Add(key, this);
            // ^ Duplicate inside TypeModel, where can we move this to that it makes sense?

            // Generic Type Parameters
            ITypeable.TestingTypeParams(type, assemblies, types);
        }                 
    }
}
