using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Core.Models.Exceptions
{
    public class RequiredTypePropertyNullException : Exception
    {
        public TypeConfig Type { get; set; }
        public string PropertyName { get; set; }

        public RequiredTypePropertyNullException(TypeConfig type, string propName, string message = null) : base(message)
        {
            Type = type;
            PropertyName = propName;
        }
    }
}
