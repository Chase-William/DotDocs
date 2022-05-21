using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using LoxSmoke.DocXml;

namespace Docsharp.Core.Models.Members
{
    public class MethodModel : Member<MethodInfo, MethodComments>, IFunctional
    {
        public override string Type => "Method";

        public string ReturnType => Meta.ReturnType.ToString();
        public Parameter[] Parameters { get; set; }

        public MethodModel(MethodInfo member) : base(member)
        {
            Parameters = ((IFunctional)this).GetParameters(member);
        }
    }
}
