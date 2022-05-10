using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Docsharp.Core.Types;

namespace Docsharp.Core.Metadata
{
    public sealed class ReflectedMetadataLoader : IDisposable
    {
        public string AssemblyName { get; private set; }
        public Dictionary<string, ClassType> Classes { get; private set; } = new();
        public Dictionary<string, StructType> Structs { get; private set; } = new();
        public Dictionary<string, InterfaceType> Interfaces { get; private set; } = new();
        public Dictionary<string, EnumType> Enumerations { get; private set; } = new();
        public Dictionary<string, DelegateType> Delegates { get; private set; } = new();

        private MetadataLoadContext mlc;

        private ReflectedMetadataLoader() { }

        public static ReflectedMetadataLoader From(string dllPath)
        {
            var meta = new ReflectedMetadataLoader();
            try
            {
                // Init context for reading member info from .dll
                meta.mlc = InitMetadataLoadContext(dllPath);
                // Get assembly reference for metadatatree
                var assembly = GetAssembly(meta.mlc, dllPath);
                // Read in .dll member info
                meta.ReadMetadataFromAssembly(assembly);
                meta.AssemblyName = assembly.FullName;
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

        private void ReadMetadataFromAssembly(Assembly assembly)
        {
            foreach (TypeInfo typeInfo in assembly.DefinedTypes)
            {
                // Sort via construct type
                if (typeInfo.BaseType?.FullName == "System.MulticastDelegate")
                    Delegates.Add(typeInfo.FullName, new DelegateType(typeInfo));
                else if (typeInfo.IsClass)
                    Classes.Add(typeInfo.FullName, new ClassType(typeInfo));
                else if (typeInfo.IsInterface)
                    Interfaces.Add(typeInfo.FullName, new InterfaceType(typeInfo));
                else if (typeInfo.IsEnum)
                    Enumerations.Add(typeInfo.FullName, new EnumType(typeInfo));
                else if (typeInfo.IsValueType) // == IsStruct
                    Structs.Add(typeInfo.FullName, new StructType(typeInfo));
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

        private static MetadataLoadContext InitMetadataLoadContext(string dllPath)
        {
            try
            {
                string[] runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");

                // Create the list of assembly paths consisting of runtime assemblies and the inspected assembly.
                var paths = new List<string>(runtimeAssemblies)
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
