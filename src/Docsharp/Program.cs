using System;
using System.Collections.Generic;
using System.IO;

using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using Docsharp.Core;
using Docsharp.Core.Types;
using Docsharp.Core.Metadata;


namespace Docsharp
{
    internal class Program
    {        
        static void Main(string[] args)
        {
            using var docs = Docsharpener.From(
                dllPath: @"C:\Dev\Sharpocs\test\Docsharp.Test.Data\bin\Debug\net5.0\Docsharp.Test.Data.dll",
                xmlPath: @"C:\Dev\Sharpocs\test\Docsharp.Test.Data\bin\Debug\net5.0\Docsharp.Test.Data.xml"
            );

            docs.MakeDocumentation();

            // ...
        }

        //private static void ReadXmlDocumentationFile()
        //{
        //    try
        //    {
        //        using var reader = new XmlTextReader(@"C:\Dev\Sharpocs\TestLibrary\bin\Debug\net5.0\TestLibrary.xml");

        //        while (reader.Read())
        //        {
        //            switch (reader.NodeType)
        //            {
        //                case XmlNodeType.Element: // Where I can read element attributes
        //                    Console.Write("<{0}>{1}", reader.Name, reader.HasAttributes ? reader.GetAttribute(0) : "");

        //                    // Break if member doesn't have a name="<type>" attribute to be valid
        //                    string? attrTest = reader.GetAttribute("name");
        //                    if (attrTest == null)
        //                        break;

        //                    string attr = attrTest;

        //                    switch (attr[0])
        //                    {
        //                        case 'T':
        //                            Console.WriteLine();
        //                            break;
        //                        case 'P':
        //                            Console.WriteLine();
        //                            break;
        //                        default:
        //                            break;
        //                    }


        //                    break;
        //                case XmlNodeType.Text: // Case for isolating the textual content of the node.
        //                    Console.Write(reader.Value);
        //                    break;
        //                //case XmlNodeType.CDATA:
        //                //    Console.Write("<![CDATA[{0}]]>", reader.Value);
        //                //    break;
        //                //case XmlNodeType.ProcessingInstruction:
        //                //    Console.Write("<?{0} {1}?>", reader.Name, reader.Value);
        //                //    break;
        //                case XmlNodeType.Comment:
        //                    Console.Write("<!--{0}-->", reader.Value);
        //                    break;
        //                case XmlNodeType.XmlDeclaration:
        //                    //Console.Write("<?xml version='1.0'?>");
        //                    break;
        //                //case XmlNodeType.Document:
        //                //    break;
        //                //case XmlNodeType.DocumentType:
        //                //    Console.Write("<!DOCTYPE {0} [{1}]", reader.Name, reader.Value);
        //                //    break;
        //                case XmlNodeType.EntityReference: // ?
        //                    Console.Write(reader.Name);
        //                    break;
        //                case XmlNodeType.EndElement:
        //                    Console.Write("</{0}>", reader.Name);
        //                    break;
        //            }
        //        } // while  
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }                                              
        //} // method

        
    }
}
