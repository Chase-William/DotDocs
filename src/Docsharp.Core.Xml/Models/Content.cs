using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace Docsharp.Core.Xml.Models
{
    public class Content
    {
        public string Text { get; set; }

        public static Content Parse(XmlReader reader)
        {
            return null;
        }
    }
}
