using DotDocs.Markdown;

using State = DotDocs.Markdown.RenderState;

namespace Test.DotDocs.Tests
{        
    [TestClass]
    public class TestTypeNaming : TypeNamingTestSource
    {
        /// <summary>
        /// Gets a string from a markdown associated with the expected value for <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of the file.</param>
        /// <param name="withLinks">Use the expected output with links.</param>
        /// <returns></returns>
        static string GetExpectedTypeNamingMarkdown(string name, bool withLinks = true)
        {
            return File.ReadAllText(Path.Combine(Extensions.ASSETS_DIR, "TypeNames", withLinks ? "WithLinks" : "NoLinks", name + Extensions.FILE_EX));
        }

        [TestCleanup]
        public void Cleanup()
            => State.Builder.Clear();        

        /// <summary>
        /// Test the handle of non-generic & non-element type standard library names.
        /// </summary>
        /// <remarks>
        /// Although a string is a char[], doesn't reveal it as a normal array
        /// </remarks>
        /// <param name="type"></param>
        [TestMethod]
        [DataRow(typeof(sbyte))]
        [DataRow(typeof(byte))]
        [DataRow(typeof(short))]
        [DataRow(typeof(ushort))]
        [DataRow(typeof(int))]
        [DataRow(typeof(uint))]
        [DataRow(typeof(long))]
        [DataRow(typeof(ulong))]
        [DataRow(typeof(char))]
        [DataRow(typeof(float))]
        [DataRow(typeof(double))]
        [DataRow(typeof(decimal))]
        [DataRow(typeof(bool))]
        [DataRow(typeof(string))]
        public void TestNamingOfStandardTypes(Type type)
        {
            type.PutTypeName();
            Assert.AreEqual($"`{type.Name}`", State.Builder.ToString());
        }

        [TestMethod("Type names without links.")]
        [DynamicData(
            nameof(GetNames),
            typeof(TypeNamingTestSource),
            DynamicDataSourceType.Method)]
        public void TestNamingOfCustomTypesNoLinks(string name)
        {
            var type = Get(name);
            type.PutTypeName();

            Assert.AreEqual(
                GetExpectedTypeNamingMarkdown(name, false),
                State.Builder.ToString());
        }

        [TestMethod]
        [DynamicData(
            nameof(GetNames),
            typeof(TypeNamingTestSource),
            DynamicDataSourceType.Method)]
        public void TestNamingOfCustomTypesWithLinks(string name)
        {
            var type = Get(name);
            // Provide a declaring type which is the source where all these types were pulled from
            type.PutTypeName(Src);

            Assert.AreEqual(
                GetExpectedTypeNamingMarkdown(name, true), 
                State.Builder.ToString());
        }
    }
}
