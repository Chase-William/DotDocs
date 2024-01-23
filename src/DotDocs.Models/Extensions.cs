using DotDocs.Models.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DotDocs.Models
{
    internal static class Extensions
    {
        public static string GetUniqueName(this Type type)
            => GetUniqueNameImpl(type.Namespace, type.Name);
        public static string GetUniqueName(this ITypeable model)
            => GetUniqueNameImpl(model.Namespace, model.Name);

        static string GetUniqueNameImpl(string _namespace, string name)
            => $"{_namespace}.{name}";
    }
}
