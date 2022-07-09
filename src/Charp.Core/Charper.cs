using System;
using Charp.Core.Loaders;
using Charp.Core.Tree;

namespace Charp.Core
{
    /// <summary>
    /// The main hub for Charp.Core.
    /// </summary>
    public sealed class Charper : IDisposable
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
        /// Destination directory for JSON.
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="Charper"/>.
        /// </summary>
        private Charper() { }

        /// <summary>
        /// Cleanup unmanaged resources linked with <see cref="ReflectedMetadata"/>.
        /// </summary>
        public void Dispose()
            => ReflectedMetadata.Dispose();

        /// <summary>
        /// Initializes a new instance of <see cref="Charper"/> loaded with data.
        /// </summary>
        /// <param name="dllPath">Dynamic Link Library to read from.</param>
        /// <param name="xmlPath">XML with comments to read from.</param>
        /// <param name="outputPath">Location for JSON output.</param>
        /// <returns>Instance of <see cref="Charper"/>.</returns>
        public static Charper From(
            string dllPath, 
            string xmlPath,
            string outputPath)
        {
            var docs = new Charper
            {
                OutputPath = outputPath
            };

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

        /// <summary>
        /// Begins the process of writing all acquired information to JSON files.
        /// </summary>
        public void Save()
            => Models.SaveModels(OutputPath);             
    }
}
