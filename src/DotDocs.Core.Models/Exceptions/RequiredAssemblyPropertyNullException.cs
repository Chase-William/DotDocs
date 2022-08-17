using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Core.Models.Exceptions
{
    public class RequiredAssemblyPropertyNullException : Exception
    {
        public Assembly Assembly { get; set; }
        public string PropertyName { get; set; }

        public RequiredAssemblyPropertyNullException(Assembly asm, string propName, string message = null) : base(message)
        {
            Assembly = asm;
            PropertyName = propName;
        }
    }
}
