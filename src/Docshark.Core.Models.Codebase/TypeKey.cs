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
        public string ForeignKey { get; set; }
        public bool IsGenericParameter { get; set; }

        public static TypeKey? From(Type? info)
        {
            // Return null if no type instance
            if (info == null)
                return null;

            var key = new TypeKey()
            {
                ForeignKey = info.GetPrimaryKey()
            };

            // Save whether this is a generic type parameter or not
            key.IsGenericParameter = info.IsGenericParameter;

            return key;
        }
    }
}
