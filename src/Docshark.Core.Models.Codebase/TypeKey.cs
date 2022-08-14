using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// in own library?

namespace Docshark.Core.Models
{
    public class TypeKey
    {
        public string ForeignKey { get; internal set; }
        public bool IsGeneric { get; internal set; }

        public static TypeKey? From(Type? info)
        {
            // Return null if no type instance
            if (info == null)
                return null;

            var key = new TypeKey()
            {
                ForeignKey = info.GetPrimaryKey()
            };

            // If the type is generic, mark the typekey as generic
            if (info.IsGenericTypeDefinition)
                key.IsGeneric = true;

            return key;
        }
    }
}
