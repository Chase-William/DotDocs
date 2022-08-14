using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// in own library?

namespace Docshark.Core.Models
{
    public static class KeyExtensions
    {
        public static string GetPrimaryKey(this Assembly assembly)
            => assembly.GetName().Name;

        public static string GetPrimaryKey(this Type type)
        {
            if (type.IsGenericParameter)
                return $"{type.Name}={type.BaseType}";
            return type.ToString();
        }
    }
}
