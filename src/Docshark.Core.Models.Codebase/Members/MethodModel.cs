using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using LoxSmoke.DocXml;
using Docshark.Core.Models.Codebase;
using System.Linq.Expressions;

namespace Docshark.Core.Models.Codebase.Members
{
    public class MethodModel : Model<MethodInfo, CommonComments>, IFunctional, IAccessible
    {
        private bool triedToGetType;
        public override string? Type
        {
            get
            {
                if (triedToGetType)
                    return MethodType?.ToString();

                triedToGetType = true;
                MethodType = GetSignature(Meta);
                return MethodType?.ToString();
            }
        }
        [System.Text.Json.Serialization.JsonIgnore]
        public Type? MethodType { get; private set; }

        public string ReturnType => Meta.ReturnType.ToString();
        public Parameter[] Parameters { get; set; }
        public bool IsVirtual => Meta.IsVirtual && !IsAbstract;        
        public bool IsAbstract => Meta.IsAbstract;
        public bool IsStatic => Meta.IsStatic;
        
        Type GetSignature(MethodInfo methodInfo)
        {
            Func<Type[], Type> getType;
            var types = methodInfo.GetParameters().Select(p => p.ParameterType);
            if (types.Any(p => p.IsByRef))
                return null; // Not supported atm
            if (methodInfo.ReturnType.FullName == typeof(void).FullName)                
            {
                getType = Expression.GetActionType;
            }
            else
            {                    
                getType = Expression.GetFuncType;
                types = types.Concat(new[] { methodInfo.ReturnType });
            }
            return getType(types.ToArray());     
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
