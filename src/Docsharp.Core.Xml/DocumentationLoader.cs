using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

using Docsharp.Core.Xml.Models;

namespace Docsharp.Core.Xml
{
    public static class XmlDocLoader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Entity[] Parse(string xmlFilePath)
        {
            try
            {
                using var reader = new XmlTextReader(xmlFilePath);

                reader.ReadToDescendant("members");
                // Iterate over all members within collection
                while (reader.ReadToDescendant("member"))
                {
                    var entity = Entity.Parse(reader);
                } 
            }
            catch
            {
                throw;
            }

            throw new NotImplementedException();
        }
    }
}
