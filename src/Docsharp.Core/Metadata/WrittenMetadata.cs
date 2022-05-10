using System;
using System.Xml;

#nullable enable

namespace Docsharp.Core.Metadata
{
    public class WrittenMetadata
    {
        public string AssemblyName { get; private set; } = string.Empty;

        private WrittenMetadata() { }

        public static WrittenMetadata? From(string filePath)
        {
            WrittenMetadata docs = new();

            try
            {
                using var reader = new XmlTextReader(filePath);

                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // Where I can read element attributes
                            //Console.Write("<{0}>{1}"s, reader.Name, reader.HasAttributes ? reader.GetAttribute(0) : "");

                            // Captures the assembly's name
                            if (reader.Name == "name")
                                docs.AssemblyName = reader.ReadElementContentAsString();

                            // Break if member doesn't have a name="<type>" attribute to be valid
                            string? attrTest = reader.GetAttribute("name");
                            if (attrTest == null)
                                break;

                            string attr = attrTest;

                            switch (attr[0])
                            {
                                case 'T':
                                    Console.WriteLine();
                                    break;
                                case 'P':
                                    Console.WriteLine();
                                    break;
                                default:
                                    break;
                            }


                            break;
                        case XmlNodeType.Text: // Case for isolating the textual content of the node.                            
                            Console.Write(reader.Value);
                            break;
                        //case XmlNodeType.CDATA:
                        //    Console.Write("<![CDATA[{0}]]>", reader.Value);
                        //    break;
                        //case XmlNodeType.ProcessingInstruction:
                        //    Console.Write("<?{0} {1}?>", reader.Name, reader.Value);
                        //    break;
                        case XmlNodeType.Comment:
                            Console.Write("<!--{0}-->", reader.Value);
                            break;
                            //case XmlNodeType.XmlDeclaration:
                            //    Console.Write("<?xml version='1.0'?>");
                            //    break;
                            //case XmlNodeType.Document:
                            //    break;
                            //case XmlNodeType.DocumentType:
                            //    Console.Write("<!DOCTYPE {0} [{1}]", reader.Name, reader.Value);
                            //    break;
                            //case XmlNodeType.EntityReference: // ?
                            //    Console.Write(reader.Name);
                            //    break;
                            //case XmlNodeType.EndElement:
                            //    Console.Write("</{0}>", reader.Name);
                            //    break;
                    }
                } // while
                return docs;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
