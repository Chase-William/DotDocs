using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown
{
    public static class Extensions
    {
        public static string GetKey(this Assembly assembly)
            => assembly.FullName ?? throw new Exception($"Assembly at {assembly.Location} does not have a valid FullName property.");
    }
}
