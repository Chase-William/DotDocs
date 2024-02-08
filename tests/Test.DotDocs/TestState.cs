using DotDocs.IO.Routing;
using DotDocs.IO;
using DotDocs.Markdown;
using DotDocs;
using DotDocs.Models;
using System.Text;
using System.Diagnostics;

namespace Test.DotDocs
{
    /// <summary>
    /// Provides state used by tests.
    /// </summary>
    [TestClass]
    public static class TestState
    {
        public static Builder? Builder { get; private set; }

        /// <summary>
        /// Gets a <see cref="Type"/> from the <see cref="Builder.Renderer"/>'s list of assemblies.
        /// </summary>
        /// <remarks>
        /// The usage of a testing type directly from the test's project dependency list is forbidden as <see cref="Type"/>  has a different concrete implementation when loaded from a <c>MetadataLoadContext</c> compared the normal loading of .NET assembly dependencies. Failure to adhere will result in seemly interchangable <see cref="Type"/> instances representing the same <see cref="Type"/> that yet fail whe compared dependending on the method for comparison used.
        /// </remarks>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Type GetECMADefinitionTypeFromRuntimeType(Type type)
        {
            Debug.Assert(Builder is not null, "Build should be initialized.");

            if (type.IsGenericParameter)
                throw new ArgumentException($"Invalid {type} given, cannot be a generic parameter.");
            // Get ref to matching assembly but loaded by MetadataLoadContext
            var reflectionAssembly = Builder.Renderer.Assemblies.FirstOrDefault(asm => asm.binary.FullName == type.Assembly.FullName);
            Debug.Assert(reflectionAssembly.binary is not null, $"Could not locate a matching assembly for {type.FullName} within renderer's assemblies.");
            // Get matching type that resides within reflection only assembly
            var ecmaType = reflectionAssembly.binary.ExportedTypes.FirstOrDefault(t => t.FullName == type.FullName);
            Debug.Assert(ecmaType is not null, $"Type {type.FullName ?? type.Name} should exist within the renderer's assemlbies.");

            return ecmaType;
        }

        public static Builder GetBuilder()
        {
            var builder = Builder.FromPath("../../../../Data", "docs");
            builder.Prepare();
            builder.Build();

            return builder;
        }        

        [AssemblyInitialize]
        public static void Init(TestContext _)
        {
            Builder = GetBuilder();
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
            Builder?.Dispose();
        }
    }
}
