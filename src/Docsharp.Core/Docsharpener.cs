using System;
using Docsharp.Core.Loaders;
using Docsharp.Core.Tree;

namespace Docsharp.Core
{
    public sealed class Docsharpener : IDisposable
    {        
        /// <summary>
        /// A tree that organizes all types.
        /// </summary>
        public ModelTree Models { get; private set; }
        /// <summary>
        /// Contains all reflection based metadata.
        /// </summary>
        public MetadataLoader ReflectedMetadata { get; private set; }
        /// <summary>
        /// Contains all human-written based metadata.
        /// </summary>
        //public Entity[] Documentation { get; private set; }
        
        private Docsharpener() { }

        public void Dispose()
            => ReflectedMetadata.Dispose();

        public static Docsharpener From(string dllPath, string xmlPath)
        {
            var docs = new Docsharpener();            

            try
            {
                docs.ReflectedMetadata = MetadataLoader.From(dllPath, xmlPath);
                // Read in .xml documentation to be joined with member info
                // docs.Documentation = XmlDocLoader.Parse(xmlPath);              
                // Create an organized structure called a MetadataTree to represent .dll type structure
                docs.Models = new ModelTree();

                /**
                 * Add all types to MetadataTree
                 */

                // Classes
                foreach (var item in docs.ReflectedMetadata.Classes)
                    docs.Models.AddType(item.Key, item.Value);

                // Interfaces
                foreach (var item in docs.ReflectedMetadata.Interfaces)
                    docs.Models.AddType(item.Key, item.Value);

                // Structs
                foreach (var item in docs.ReflectedMetadata.Structs)
                    docs.Models.AddType(item.Key, item.Value);

                // Enumerations
                foreach (var item in docs.ReflectedMetadata.Enumerations)
                    docs.Models.AddType(item.Key, item.Value);

                // Delegates
                foreach (var item in docs.ReflectedMetadata.Delegates)
                    docs.Models.AddType(item.Key, item.Value);

                return docs;
            }
            catch
            {
                throw;
            }            
        }

        public void Save()
            => Models.SaveModels();             
    }
}
