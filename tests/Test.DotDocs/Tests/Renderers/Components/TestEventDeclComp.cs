using DotDocs.Markdown.Enums;
using DotDocs.Markdown.Renderers.Components;
using DotDocs.Markdown;

using System.Reflection;

namespace Test.DotDocs.Tests.Renderers.Components
{
    [TestClass]
    public class TestEventDeclComp : Component<EventDeclaration, EventInfo, EventTestSource>
    {
        [TestMethod]
        [DynamicData(
            nameof(EventTestSource.GetNames),
            typeof(EventTestSource),
            DynamicDataSourceType.Method)]
        public void EventDeclarationCompontentTest(string name)
        {
            var _event = src.Get(name);
            Assert.IsNotNull(_event);

            renderer.Render(_event, Padding.None);
            Assert.AreEqual(
                _event.GetExpectedCompMarkdown(renderer),
                RenderState.Builder.ToString().ReplaceLineEndings());
        }
    }
}
