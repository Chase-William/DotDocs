using System;
using Docsharp.Core.Loaders;
using Docsharp.Core.Xml;
using Docsharp.Core.Xml.Models;
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
        public ReflectedMetadataLoader ReflectedMetadata { get; private set; }
        /// <summary>
        /// Contains all human-written based metadata.
        /// </summary>
        public Entity[] Documentation { get; private set; }
        
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
                docs.Documentation = XmlDocLoader.Parse(xmlPath);              
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


                // Add written documentation to each type/member
                //foreach (var document in docs.WrittenMetadata.Documentation)
                //{
                //    switch (document.Type)
                //    {
                //        case MemberType.Type:
                //            var type = docs.Models.FindType(document.FullName);
                //            type.Docs = document;
                //            break;
                //        case MemberType.Field:
                //            var field = docs.Models.FindField(document.FullName);
                //            field.Docs = document;
                //            break;
                //        case MemberType.Property:
                //            var property = docs.Models.FindProperty(document.FullName);
                //            property.Docs = document;
                //            break;
                //        case MemberType.Event:
                //            var _event = docs.Models.FindEvent(document.FullName);
                //            _event.Docs = document;
                //            break;
                //        default:
                //            break;
                //    }
                //}

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
