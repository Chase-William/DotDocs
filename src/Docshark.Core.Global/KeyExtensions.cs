using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Core.Global
{
    internal static class KeyExtensions
    {
        public static string GetPrimaryKey(this Assembly assembly)
            => assembly.GetName().Name;

        public static string GetPrimaryKey(this Type type)
            => type.ToString();
    }
}
