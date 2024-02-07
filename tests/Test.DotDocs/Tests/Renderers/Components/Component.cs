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
    /// <summary>
    /// A base <c>class</c> all component based test can use.
    /// </summary>
    /// <typeparam name="TComp"></typeparam>
    /// <typeparam name="TMemberInfo"></typeparam>
    /// <typeparam name="TDataSource"></typeparam>
    public abstract class Component<TComp, TMemberInfo, TDataSource>        
        where TComp : IComponentRenderer<TMemberInfo>, new()
        where TMemberInfo : MemberInfo
        where TDataSource : MemberTestSource<TMemberInfo>, new()
    {
        #region Boilerplate Test Setup
        protected TComp renderer;
        protected TDataSource src;

        [TestInitialize]
        public void Setup()
        {
            src = new TDataSource();
            renderer = new TComp();
            RenderState.Builder.Clear();
        }
        #endregion
    }
}
