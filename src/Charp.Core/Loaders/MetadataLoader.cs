using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Charp.Core.Models.Types;
using LoxSmoke.DocXml;

namespace Charp.Core.Loaders
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

        public static MetadataLoader From(string csProjPath, string dllPath, string xmlPath)
        {
            var meta = new MetadataLoader();
            try
            {
                // Init context for reading member info from .dll
                meta.mlc = InitMetadataLoadContext(csProjPath, dllPath);
                // Get assembly reference for metadatatree
                var assembly = GetAssembly(meta.mlc, dllPath);
                // Read in .dll member info
                meta.ResolveMetadata(assembly, xmlPath);
                meta.AssemblyName = assembly.GetName().Name;
                meta.FullAssemblyName = assembly.FullName;
            }
            catch
            {
                throw;
            }
            return meta;
        }

        public void Dispose()
        {
            mlc.Dispose();
        }

        private void ResolveMetadata(Assembly assembly, string xmlPath)
        {
            DocXmlReader reader = new DocXmlReader(xmlPath);
            TypeComments comments = null;
            foreach (TypeInfo typeInfo in assembly.DefinedTypes)
            {
                comments = reader.GetTypeComments(typeInfo);
                // Sort via construct type
                if (typeInfo.BaseType?.FullName == "System.MulticastDelegate")                
                    Delegates.Add(typeInfo.FullName, new DelegateModel(typeInfo)
                    {
                        Comments = comments
                    });                
                else if (typeInfo.IsClass)                
                    Classes.Add(typeInfo.FullName, new ClassModel(typeInfo, reader)
                    {
                        Comments = comments
                    });               
                else if (typeInfo.IsInterface)
                    Interfaces.Add(typeInfo.FullName, new InterfaceModel(typeInfo, reader)
                    {
                        Comments = comments
                    });
                else if (typeInfo.IsEnum)
                    Enumerations.Add(typeInfo.FullName, new EnumModel(typeInfo, reader)
                    {
                        Comments = comments
                    });
                else if (typeInfo.IsValueType) // == IsStruct
                    Structs.Add(typeInfo.FullName, new StructModel(typeInfo, reader)
                    {
                        Comments = comments
                    });
            }
        }

        private static Assembly GetAssembly(MetadataLoadContext mlc, string dllPath)
        {
            try
            {
                return mlc.LoadFromAssemblyPath(dllPath);
            }
            catch
            {
                throw;
            }
        }

        private static MetadataLoadContext InitMetadataLoadContext(string csProjPath, string dllPath)
        {            
            try
            {
                string[] runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");

                var deps = DependencyRetriever.FindAll(csProjPath, runtimeAssemblies);

                // Create the list of assembly paths consisting of runtime assemblies and the inspected assembly.
                var paths = new List<string>(deps)
                {
                    dllPath                   
                };

                // Create PathAssemblyResolver that can resolve assemblies using the created list.
                var resolver = new PathAssemblyResolver(paths);

                // KEEP CONNECTION UNTIL END OF PROGRAM
                return new MetadataLoadContext(resolver, "System.Private.CoreLib");
            }
            catch
            {
                throw;
            }
        }
    }
}
