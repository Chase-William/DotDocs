using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.IO.Routing
{
    /// <summary>
    /// Output each type within a folder directory representing its namespace.
    /// </summary>
    public class DefaultRouter : IRouterable
    {
        public string GetName(Type type)
             => type.Name;

        public string GetLocation(Type type)
            // Join an array of strings taken from the splitting of the fullname where we took from 0 to len - 1 exclusive
            => string.Join('/', (type.FullName ?? throw new Exception($"Type {type} has null Fullname property.")).Split('.')[..^1]);

        public string GetRoute(Type from, Type to)
        {
            return Path.GetRelativePath(GetLocation(from), GetLocation(to));
        }
    }
}
