using System.Reflection;
using LoxSmoke.DocXml;
using DotDocs.Core.Models.Language.Parameters;
using DotDocs.Core.Models.Language.Interfaces;

namespace DotDocs.Core.Models.Language.Members
{
    public class MethodModel : MemberModel<MethodInfo, CommonComments>, IHaveSignature
    {
        public bool IsPublic => Info.IsPublic;
        public bool IsProtected => Info.IsFamily || Info.IsFamilyOrAssembly || Info.IsFamilyAndAssembly;
        public bool IsInternal => Info.IsAssembly || Info.IsFamilyOrAssembly;
        public bool IsPrivate => Info.IsPrivate;

        ParameterModel returnParameter;
        public ParameterModel ReturnParameter
            => returnParameter ??= new ParameterModel(Info.ReturnParameter);

        ParameterModel[] parameters;
        public ParameterModel[] Parameters
            => parameters ??= ((IHaveSignature)this).GetParameters(Info);

        public bool IsVirtual => Info.IsVirtual && !IsAbstract;
        public bool IsAbstract => Info.IsAbstract;
        public bool IsStatic => Info.IsStatic;
        
        public MethodModel(MethodInfo info) : base(info) { }
    }
}
