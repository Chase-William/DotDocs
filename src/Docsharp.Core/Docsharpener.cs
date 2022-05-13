using System;

using Docsharp.Core.Metadata;
using Docsharp.Core.Tree;

namespace Docsharp.Core
{
    public sealed class Docsharpener : IDisposable
    {        
        /// <summary>
        /// A tree that organizes all types.
        /// </summary>
        public ModelTree ModelTree { get; private set; }
        /// <summary>
        /// Contains all reflection based metadata.
        /// </summary>
        public ReflectedMetadataLoader ReflectedMetadata { get; private set; }
        /// <summary>
        /// Contains all human-written based metadata.
        /// </summary>
        public WrittenDocumentationLoader WrittenMetadata { get; private set; }
        
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
                docs.WrittenMetadata = WrittenDocumentationLoader.From(xmlPath);                
                // Create an organized structure called a MetadataTree to represent .dll type structure
                docs.ModelTree = new ModelTree();

                /**
                 * Add all types to MetadataTree
                 */

                // Classes
                foreach (var item in docs.ReflectedMetadata.Classes)
                    docs.ModelTree.AddType(item.Key, item.Value);

                // Interfaces
                foreach (var item in docs.ReflectedMetadata.Interfaces)
                    docs.ModelTree.AddType(item.Key, item.Value);

                // Structs
                foreach (var item in docs.ReflectedMetadata.Structs)
                    docs.ModelTree.AddType(item.Key, item.Value);

                // Enumerations
                foreach (var item in docs.ReflectedMetadata.Enumerations)
                    docs.ModelTree.AddType(item.Key, item.Value);

                // Delegates
                foreach (var item in docs.ReflectedMetadata.Delegates)
                    docs.ModelTree.AddType(item.Key, item.Value);
                
                return docs;
            }
            catch
            {
                throw;
            }            
        }

        public void Save()
            => ModelTree.SaveModels();        

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
