using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using LoxSmoke.DocXml;
using Docshark.Core.Models.Lang;

namespace Docshark.Core.Models.Lang.Members
{
    public class MethodModel : Model<MethodInfo, CommonComments>, IFunctional, IAccessible
    {
        public override string Type => "method";
        public string ReturnType => Meta.ReturnType.ToString();
        public Parameter[] Parameters { get; set; }
        public bool IsVirtual => Meta.IsVirtual && !IsAbstract;        
        public bool IsAbstract => Meta.IsAbstract;
        public bool IsStatic => Meta.IsStatic;

        #region IAccessible
        public bool IsPublic => Meta.IsPublic;        
        public bool IsProtected => Meta.IsFamily || Meta.IsFamilyOrAssembly || Meta.IsFamilyAndAssembly;
        public bool IsInternal => Meta.IsAssembly || Meta.IsFamilyOrAssembly;
        public bool IsPrivate => Meta.IsPrivate;
        #endregion

        public MethodModel(MethodInfo member) : base(member)
            => Parameters = ((IFunctional)this).GetParameters(member);        
    }
}
