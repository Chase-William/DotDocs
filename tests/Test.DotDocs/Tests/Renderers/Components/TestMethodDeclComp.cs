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
    public class TestMethodDeclComp : Component<MethodDeclaration, MethodInfo, MethodTestSource>
    {
        [TestMethod]
        [DynamicData(
            nameof(MethodTestSource.GetNames),
            typeof(MethodTestSource),
            DynamicDataSourceType.Method)]
        public void MethodDeclarationCompontentTest(string name)
        {
            var method = src.Get(name);
            Assert.IsNotNull(method);

            renderer.Render(method, Padding.None);
            Assert.AreEqual(
                method.GetExpectedCompMarkdown(renderer),
                RenderState.Builder.ToString().ReplaceLineEndings());
        }
    }
}
