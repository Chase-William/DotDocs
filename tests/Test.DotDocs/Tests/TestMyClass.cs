using DotDocs;
using DotDocs.IO.Routing;
using DotDocs.IO;
using DotDocs.Markdown;

using Test.Source.One;
using System.Reflection;
using System.Text;

namespace Test.DotDocs.Tests
{
    //[TestClass]
    //public class TestMyClass
    //{        
    //    readonly Type myClass = typeof(MyClass);

    //    [TestInitialize]
    //    public void ClearBuffer()
    //    {
    //        RenderState.Builder.Clear();
    //    }

    //    [TestMethod]
    //    public void RenderHeader()
    //    {
    //        if (!myClass.RenderSection())
    //            return;

    //        Assert.AreEqual(
    //            myClass.GetExpectedSectionMarkdown(),
    //            RenderState.Builder.ToString().ReplaceLineEndings());
    //    }

    //    [TestMethod]
    //    public void RenderFooter()
    //    {
    //        if (!myClass.RenderSection(SectionType.Footer))
    //            return;

    //        Assert.AreEqual(
    //            myClass.GetExpectedSectionMarkdown(SectionType.Footer),
    //            RenderState.Builder.ToString().ReplaceLineEndings());
    //    }

    //    [TestMethod]
    //    [DataRow("MyProperty01")]
    //    public void RenderProperty(string propName)
    //    {
    //        var prop = myClass.GetProperty(propName);
    //        Assert.IsNotNull(prop);

    //        prop.RenderMember();
    //        Assert.AreEqual(
    //            prop.GetExpectedMemberMarkdown(),
    //            RenderState.Builder.ToString().ReplaceLineEndings());
    //    }
    //}
}