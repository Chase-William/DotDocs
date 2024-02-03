using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.IO.Routing
{
    /// <summary>
    /// Output all types within the same folder.
    /// </summary>
    public class FlatRouter : IRouterable
    {
        public string GetName(Type type)
            => $"{type.Namespace}.{type.Name}";
        
        public string GetLocation(Type type)
            => string.Empty;

        public string GetRoute(Type from, Type to)
        {
            return $"./{GetName(to)}";
        }
    }
}
