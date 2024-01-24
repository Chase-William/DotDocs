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
        public string GetFileName(Type type)
            => type.FullName ?? throw new Exception($"Type {type} has null Fullname property.");

        public string GetDir(Type type)
            => string.Empty;
    }
}
