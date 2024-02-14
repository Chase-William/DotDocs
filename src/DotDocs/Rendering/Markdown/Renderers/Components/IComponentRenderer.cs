using DotDocs.Markdown.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown.Renderers.Components
{
    public interface IComponentRenderer<T>
        where T : MemberInfo
    {
        public void Render(T info, Padding padding);
    }
}
