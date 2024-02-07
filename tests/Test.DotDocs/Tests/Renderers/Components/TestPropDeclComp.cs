using DotDocs.Markdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Source.One;

using DotDocs;
using DotDocs.IO;
using DotDocs.IO.Routing;
using DotDocs.Markdown.Renderers.Components;
using DotDocs.Markdown.Enums;
using System.Reflection;

namespace Test.DotDocs.Tests.Renderers.Components
{    
    [TestClass]
    public class TestPropDeclComp : Component<PropertyDeclaration, PropertyInfo, PropertyTestSource>
    {
        [TestMethod]
        [DynamicData(
            nameof(PropertyTestSource.GetNames),
            typeof(PropertyTestSource),
            DynamicDataSourceType.Method)]
        public void PropertyDeclarationComponentTest(string name)
        {
            var prop = src.Get(name);
            Assert.IsNotNull(prop);

            renderer.Render(prop, Padding.None);
            Assert.AreEqual(
                prop.GetExpectedCompMarkdown(renderer),
                RenderState.Builder.ToString().ReplaceLineEndings());
        }
    }
}
