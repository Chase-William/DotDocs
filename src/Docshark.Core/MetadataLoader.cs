using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Docshark.Core.Models;
using Docshark.Core.Models.Lang.Types;
using LoxSmoke.DocXml;

namespace Docshark.Core.Loaders
{
    public sealed class MetadataLoader : IDisposable
    {
        /// <summary>
        /// Assembly name this <see cref="MetadataLoader"/> instance reflected on.
        /// </summary>
        public string AssemblyName { get; private set; }

        public string FullAssemblyName { get; private set; }
        
        // Stores all types via their <namespace>.<member_name>.
        
        public Dictionary<string, ClassModel> Classes { get; private set; } = new();
        public Dictionary<string, StructModel> Structs { get; private set; } = new();
        public Dictionary<string, InterfaceModel> Interfaces { get; private set; } = new();
        public Dictionary<string, EnumModel> Enumerations { get; private set; } = new();
        public Dictionary<string, DelegateModel> Delegates { get; private set; } = new();

        private MetadataLoadContext mlc;

        private MetadataLoader() { }

        public static MetadataLoader From(string targetAsmPath, string[] depAsmPaths)
        {
            var meta = new MetadataLoader();

            // Create the list of assembly paths consisting of runtime assemblies and the inspected assembly.
            var paths = new List<string>(depAsmPaths);

            // Create PathAssemblyResolver that can resolve assemblies using the created list.
            // Init context for reading member info from .dll
            meta.mlc = new MetadataLoadContext(new PathAssemblyResolver(paths));
            // Get assembly reference for metadatatree
            var assembly = meta.mlc.LoadFromAssemblyPath(targetAsmPath);
            // Read in .dll member info
            meta.ResolveMetadata(assembly, targetAsmPath.Substring(0, targetAsmPath.LastIndexOf(".")) + ".xml");
            meta.AssemblyName = assembly.GetName().Name;
            meta.FullAssemblyName = assembly.FullName;            

            return meta;
        }

        public void Dispose()
            => mlc.Dispose();        

        private void ResolveMetadata(Assembly assembly, string xmlPath)
        {
            DocXmlReader reader = new DocXmlReader(xmlPath);
            TypeComments comments;
            foreach (TypeInfo typeInfo in assembly.DefinedTypes)
            {
                comments = reader.GetTypeComments(typeInfo);
                // Sort via construct type
                if (typeInfo.BaseType?.FullName == "System.MulticastDelegate")
                {
                    var model = new DelegateModel(typeInfo)
                    {
                        Comments = comments
                    };
                    Delegates.Add(typeInfo.FullName, model);
                    TypeMetaMapper.Add(model);
                }              
                else if (typeInfo.IsClass && !typeInfo.GetCustomAttributesData().Any(attr => attr.AttributeType.Name == typeof(CompilerGeneratedAttribute).Name))
                {
                    var model = new ClassModel(typeInfo, reader)
                    {
                        Comments = comments
                    };
                    Classes.Add(typeInfo.FullName, model);
                    TypeMetaMapper.Add(model);
                }              
                else if (typeInfo.IsInterface)
                {
                    var model = new InterfaceModel(typeInfo, reader)
                    {
                        Comments = comments
                    };
                    Interfaces.Add(typeInfo.FullName, model);
                    TypeMetaMapper.Add(model);
                }
                else if (typeInfo.IsEnum)
                {
                    var model = new EnumModel(typeInfo, reader)
                    {
                        Comments = comments
                    };
                    Enumerations.Add(typeInfo.FullName, model);
                    TypeMetaMapper.Add(model);
                }
                else if (typeInfo.IsValueType) // == IsStruct
                {
                    var model = new StructModel(typeInfo, reader)
                    {
                        Comments = comments
                    };
                    Structs.Add(typeInfo.FullName, model);
                    TypeMetaMapper.Add(model);
                }                
            }
        }
    }
}
