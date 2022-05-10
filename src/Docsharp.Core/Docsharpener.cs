using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Docsharp.Core.Types;
using Docsharp.Core.Metadata;

namespace Docsharp.Core
{
    public sealed class Docsharpener : IDisposable
    {
        /// <summary>
        /// Stores all class type via their <module>.<member_name>.
        /// </summary>
        public Dictionary<string, ClassType> Classes { get; private set; } = new();
        public Dictionary<string, StructType> Structs { get; private set; } = new();
        public Dictionary<string, InterfaceType> Interfaces { get; private set; } = new();
        public Dictionary<string, EnumType> Enumerations { get; private set; } = new();
        public Dictionary<string, DelegateType> Delegates { get; private set; } = new();

        public MetadataTree Metadata { get; private set; }

        public ReflectedMetadataLoader ReflectedMetadata { get; private set; }
        public WrittenMetadataLoader WrittenMetadata { get; private set; }
        
        private Docsharpener() { }

        public void Dispose()
            => ReflectedMetadata.Dispose();

        public static Docsharpener From(string dllPath, string xmlPath)
        {
            var docs = new Docsharpener();            

            try
            {
                docs.ReflectedMetadata = ReflectedMetadataLoader.From(dllPath);
                // Read in .xml documentation to be joined with member info
                docs.WrittenMetadata = WrittenMetadataLoader.From(xmlPath);                
                // Create an organized structure called a MetadataTree to represent .dll type structure
                docs.Metadata = new MetadataTree(docs.ReflectedMetadata.AssemblyName);

                /**
                 * Add all types to MetadataTree
                 */

                // Classes
                foreach (var item in docs.Classes)
                    docs.Metadata.AddType(item.Key, item.Value);

                // Interfaces
                foreach (var item in docs.Interfaces)
                    docs.Metadata.AddType(item.Key, item.Value);

                // Structs
                foreach (var item in docs.Structs)
                    docs.Metadata.AddType(item.Key, item.Value);

                // Enumerations
                foreach (var item in docs.Enumerations)
                    docs.Metadata.AddType(item.Key, item.Value);

                // Delegates
                foreach (var item in docs.Delegates)
                    docs.Metadata.AddType(item.Key, item.Value);

                return docs;
            }
            catch
            {
                throw;
            }            
        }

        //public void MakeDocumentation()
        //{
        //    try
        //    {
        //        Directory.CreateDirectory("meta");

        //        string memStr;
        //        foreach (var member in Classes)
        //        {
        //            memStr = JsonSerializer.Serialize(member.Value);
        //            using StreamWriter writer = new("./meta/" + member.Key + ".json", false);
        //            writer.Write(memStr);
        //        }

        //        //foreach (var member in Structs)
        //        //{
        //        //    memStr = JsonSerializer.Serialize(member.Value);
        //        //    using StreamWriter writer = new(member.Key + ".json", false);
        //        //    writer.Write(memStr);
        //        //}

        //        //foreach (var member in Interfaces)
        //        //{
        //        //    memStr = JsonSerializer.Serialize(member.Value);
        //        //    using StreamWriter writer = new(member.Key + ".json", false);
        //        //    writer.Write(memStr);
        //        //}

        //        //foreach (var member in Enumerations)
        //        //{
        //        //    memStr = JsonSerializer.Serialize(member.Value);
        //        //    using StreamWriter writer = new(member.Key + ".json", false);
        //        //    writer.Write(memStr);
        //        //}
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}                      
    }
}
