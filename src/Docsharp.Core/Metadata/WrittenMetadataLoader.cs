using System;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;

namespace Docsharp.Core.Metadata
{
    public class WrittenMetadataLoader
    {
        public string AssemblyName { get; private set; } = string.Empty;

        public List<MemberDocs> Documentation { get; private set; } = new();

        private WrittenMetadataLoader() { }        

        enum XmlElements
        {
            Name,
            Member,
            Summary,
            Param,
            Returns,
            /// <summary>
            /// Element types we do not care about are marked as such.
            /// </summary>
            Ignore
        }

        /// <summary>
        /// Retrieve a <see cref="WrittenMetadataLoader"/> instance populated with metadata from the provided param.
        /// </summary>
        /// <param name="filePath">File path to .xml file.</param>
        /// <returns>Instance of <see cref="WrittenMetadataLoader"/>.</returns>
        public static WrittenMetadataLoader From(string filePath)
        {
            WrittenMetadataLoader meta = new();
            
            try
            {
                using var reader = new XmlTextReader(filePath);

                // State variables for parsing
                XmlElements current = XmlElements.Ignore;
                MemberDocs memDocs = null;
                Param param = null;
                string nameAttr = null;

                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        // State has changed to a new element
                        case XmlNodeType.Element:
                            switch (reader.Name)
                            {
                                case "name":
                                    current = XmlElements.Name;
                                    break;
                                case "member":
                                    if (memDocs != null)
                                        meta.Documentation.Add(memDocs);
                                    current = XmlElements.Member;
                                    nameAttr = reader.GetAttribute("name");
                                    memDocs = new MemberDocs
                                    {
                                        // Cast leading character to type
                                        Type = (MemberType)nameAttr[0],
                                        // Omit type and colon from FullName always
                                        FullName = nameAttr[2..nameAttr.Length]
                                    };
                                    break;
                                case "summary":
                                    current = XmlElements.Summary;
                                    break;
                                case "param":
                                    current = XmlElements.Param;
                                    param = new Param
                                    {
                                        Name = reader.GetAttribute("name")
                                    };
                                    memDocs.Params.Add(param);
                                    break;
                                case "returns":
                                    current = XmlElements.Returns;
                                    break;
                                default:
                                    current = XmlElements.Ignore;
                                    break;
                            }

                            Console.WriteLine(reader.Value);
                            break;
                        // Contents of current node state
                        case XmlNodeType.Text:                         
                            switch (current)
                            {
                                case XmlElements.Name:
                                    meta.AssemblyName = reader.Value;
                                    break;
                                case XmlElements.Summary:
                                    memDocs.Summary = reader.Value;
                                    break;
                                case XmlElements.Param:
                                    param.Body = reader.Value; 
                                    break;
                                case XmlElements.Returns:
                                    memDocs.Returns = reader.Value;
                                    break;
                                default:                                    
                                    break;
                            }
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
                return meta;
            }
            catch
            {
                throw;
            }
        }        
    }
}
