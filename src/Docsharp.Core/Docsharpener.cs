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
    public class Docsharpener : IDisposable
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

        private MetadataLoadContext mlc;

        private Docsharpener() { }

        public static Docsharpener From(string dllPath, string xmlPath)
        {
            var docSharpener = new Docsharpener();

            try
            {
                docSharpener.mlc = InitMetadataLoadContext(dllPath);

                var assembly = GetAssembly(docSharpener.mlc, dllPath);

                docSharpener.ReadMetadataFromAssembly(assembly);
                // var docs = WrittenMetadata.From(xmlPath);
                // docSharpener.WriteDocumentationToFile();

                docSharpener.Metadata = new MetadataTree(assembly.GetName().Name);

                // Classes
                foreach (var item in docSharpener.Classes)
                    docSharpener.Metadata.AddType(item.Key, item.Value);

                // Interfaces
                foreach (var item in docSharpener.Interfaces)
                    docSharpener.Metadata.AddType(item.Key, item.Value);

                // Structs
                foreach (var item in docSharpener.Structs)
                    docSharpener.Metadata.AddType(item.Key, item.Value);

                // Enumerations
                foreach (var item in docSharpener.Enumerations)
                    docSharpener.Metadata.AddType(item.Key, item.Value);

                // Delegates
                foreach (var item in docSharpener.Delegates)
                    docSharpener.Metadata.AddType(item.Key, item.Value);



                return docSharpener;
            }
            catch
            {
                throw;
            }            
        }

        public void MakeDocumentation()
        {
            try
            {
                Directory.CreateDirectory("meta");

                string memStr;
                foreach (var member in Classes)
                {
                    memStr = JsonSerializer.Serialize(member.Value);
                    using StreamWriter writer = new("./meta/" + member.Key + ".json", false);
                    writer.Write(memStr);
                }

                //foreach (var member in Structs)
                //{
                //    memStr = JsonSerializer.Serialize(member.Value);
                //    using StreamWriter writer = new(member.Key + ".json", false);
                //    writer.Write(memStr);
                //}

                //foreach (var member in Interfaces)
                //{
                //    memStr = JsonSerializer.Serialize(member.Value);
                //    using StreamWriter writer = new(member.Key + ".json", false);
                //    writer.Write(memStr);
                //}

                //foreach (var member in Enumerations)
                //{
                //    memStr = JsonSerializer.Serialize(member.Value);
                //    using StreamWriter writer = new(member.Key + ".json", false);
                //    writer.Write(memStr);
                //}
            }
            catch
            {
                throw;
            }
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

        public void Dispose()
        {
            mlc.Dispose();
        }
    }
}
