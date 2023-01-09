using DotDocs.Core.Comments;
using DotDocs.Core.Language.Interfaces;
using DotDocs.Core.Language.Parameters;
using System.Reflection;

namespace DotDocs.Core.Language.Members
{
    public class MethodModel : MemberModel<MethodInfo, MethodCommentsModel>, IHaveSignature
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
