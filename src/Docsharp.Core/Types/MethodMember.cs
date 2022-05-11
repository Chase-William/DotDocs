using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using Docsharp.Core.Metadata;

namespace Docsharp.Core.Types
{
    public class MethodMember : Member<MethodInfo, FunctionalDocumentation>, IFunctional
    {
        public override string Type => "Method";

        public string ReturnType => TypeInfo.ReturnType.ToString();
        public Parameter[] Parameters { get; set; }

        public MethodMember(MethodInfo member) : base(member)
        {
            Parameters = ((IFunctional)this).GetParameters(member);
        }
    }
}
