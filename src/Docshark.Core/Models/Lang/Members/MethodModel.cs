using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using LoxSmoke.DocXml;
using Docshark.Core.Models.Lang;
using System.Linq.Expressions;

namespace Docshark.Core.Models.Lang.Members
{
    public class MethodModel : Model<MethodInfo, CommonComments>, IFunctional, IAccessible
    {
        public override string Type => GetSignature(Meta, this);
        public string ReturnType => Meta.ReturnType.ToString();
        public Parameter[] Parameters { get; set; }
        public bool IsVirtual => Meta.IsVirtual && !IsAbstract;        
        public bool IsAbstract => Meta.IsAbstract;
        public bool IsStatic => Meta.IsStatic;

        static string GetSignature(MethodInfo methodInfo, object target)
        {
            Func<Type[], Type> getType;
            var types = methodInfo.GetParameters().Select(p => p.ParameterType);
            if (methodInfo.ReturnType.FullName == typeof(void).FullName)            
                getType = Expression.GetActionType;            
            else
            {
                getType = Expression.GetFuncType;
                types = types.Concat(new[] { methodInfo.ReturnType });
            }
            return getType(types.ToArray()).ToString();
        }

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
