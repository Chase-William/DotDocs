using DotDocs.Markdown.Enums;
using DotDocs.Markdown.Renderers.Components;
using DotDocs.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Test.DotDocs.Tests.Renderers.Components
{
    [TestClass]
    public class TestFieldDeclComp : Component<FieldDeclaration, FieldInfo, FieldTestSource>
    {
        [TestMethod]
        [DynamicData(
            nameof(FieldTestSource.GetNames),
            typeof(FieldTestSource),
            DynamicDataSourceType.Method)]
        public void FieldDeclarationCompontentTest(string name)
        {
            var field = src.Get(name);
            Assert.IsNotNull(field);

            renderer.Render(field, Padding.None);
            Assert.AreEqual(
                field.GetExpectedCompMarkdown(renderer),
                RenderState.Builder.ToString().ReplaceLineEndings());
        }
    }
}
