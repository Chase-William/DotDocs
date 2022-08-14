using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Core.Models.Codebase
{
    public class TypeKeyParameter : TypeKey
    {
        public string ParamName { get; set; }

        public static TypeKeyParameter From(ParameterInfo info)
        {
            // Return null if no type instance
            if (info == null)
                return null;

            var key = new TypeKeyParameter()
            {
                ForeignKey = info.ParameterType.GetPrimaryKey(),
                ParamName = info.Name
            };

            // If the type is generic, mark the typekey as generic
            if (info.ParameterType.IsGenericParameter)
                key.IsGenericParameter = info.ParameterType.IsGenericParameter;

            return key;
        }
    }
}
