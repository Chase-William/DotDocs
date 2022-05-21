using Docsharp.Core.Xml.Enumerations;
using System;
using System.Collections.Generic;

using System.Xml;

namespace Docsharp.Core.Xml.Models
{
    public class Entity
    {
        public string FullName { get; private set; }

        /* 
        Entity type is the type of the member in xml         
        I decided to make the typedef 'EntityType' instead of 'MemberType' because         
        the prior of the two names makes more sense to someone using this library         
        externally.
        */
        public EntityType Type { get; private set; }

        public List<Content> Members { get; private set; } = new();

        protected Entity() { }

        public static Entity Parse(XmlTextReader reader)
        {
            Entity e = new();

            var nameAttr = reader.GetAttribute("name");
            // Cast leading character to type
            e.Type = (EntityType)nameAttr[0];
            // Omit type and colon from FullName always
            e.FullName = nameAttr[2..];

            // TODO: Parse xml structure

            if (reader.ReadToDescendant("summary"))
            {
                //e.Members.Add(Content.Parse(reader));
                Console.WriteLine();
            }

            reader.ReadStartElement();

            Console.WriteLine();

            //if (reader.ReadStartElement())
            //{
            //    Console.WriteLine();
            //}

            //if (reader.)

            // Change later
            throw new Exception($"Failed to read <summary> tag from {nameof(Entity)}.Parse implemenation.");
        }
    }
}
