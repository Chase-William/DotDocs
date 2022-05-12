using System;
using System.Collections.Generic;
using System.Xml;

using Docsharp.Core.Models.Docs;

namespace Docsharp.Core
{
    public class WrittenDocumentationLoader
    {
        public string AssemblyName { get; private set; } = string.Empty;

        public List<Documentation> Documentation { get; private set; } = new();

        private WrittenDocumentationLoader() { }

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
        /// Retrieve a <see cref="WrittenDocumentationLoader"/> instance populated with metadata from the provided param.
        /// </summary>
        /// <param name="filePath">File path to .xml file.</param>
        /// <returns>Instance of <see cref="WrittenDocumentationLoader"/>.</returns>
        public static WrittenDocumentationLoader From(string filePath)
        {
            WrittenDocumentationLoader docs = new();

            try
            {
                using var reader = new XmlTextReader(filePath);

                // State variables for parsing
                XmlElements current = XmlElements.Ignore;
                Documentation memDocs = null;
                FunctionalDocumentation pMemDocs = null;
                ParamDocumentation param = null;
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
                                    {
                                        docs.Documentation.Add(memDocs);
                                        memDocs = null;
                                    }
                                    else
                                    {
                                        docs.Documentation.Add(pMemDocs);
                                        pMemDocs = null;
                                    }

                                    current = XmlElements.Member;
                                    nameAttr = reader.GetAttribute("name");
                                    memDocs = new Documentation
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
                                    // Upgrade type if not already to store "parameter" info
                                    UpgradeDocType(ref memDocs, ref pMemDocs);
                                    param = new ParamDocumentation
                                    {
                                        Name = reader.GetAttribute("name")
                                    };
                                    pMemDocs.Params.Add(param);
                                    break;
                                case "returns":
                                    // Upgrade type if not already to store "returns" info
                                    UpgradeDocType(ref memDocs, ref pMemDocs);
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
                                    docs.AssemblyName = reader.Value;
                                    break;
                                case XmlElements.Summary:
                                    memDocs.Summary = reader.Value;
                                    break;
                                case XmlElements.Param:
                                    param.Summary = reader.Value;
                                    break;
                                case XmlElements.Returns:
                                    pMemDocs.Returns = reader.Value;
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
                return docs;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Upgrades <paramref name="memDocs"/> to the type of <see cref="FunctionalDocumentation"/> and 
        /// assigns it to <paramref name="pMemDocs"/>.
        /// </summary>
        /// <param name="memDocs">To be upgraded.</param>
        /// <param name="pMemDocs">To contain upgrade.</param>
        private static void UpgradeDocType(ref Documentation memDocs, ref FunctionalDocumentation pMemDocs)
        {
            if (pMemDocs == null)
            {
                pMemDocs = new FunctionalDocumentation
                {
                    Type = memDocs.Type,
                    FullName = memDocs.FullName,
                    Summary = memDocs.Summary
                };
                memDocs = null;
            }
        }
    }
}
