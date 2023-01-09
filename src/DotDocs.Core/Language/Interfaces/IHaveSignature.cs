using DotDocs.Core.Language.Parameters;
using System.Reflection;

namespace DotDocs.Core.Language.Interfaces
{
    /// <summary>
    /// Represents a type that has functional based documentation like parameters and return type.
    /// </summary>
    public interface IHaveSignature
    {
        public ParameterModel ReturnParameter { get; }

        /// <summary>
        /// Parameters used in the function signature.
        /// </summary>
        public ParameterModel[] Parameters { get; }

        /// <summary>
        /// Gets all the parameter information and organizes it before returning it.
        /// </summary>
        /// <param name="info">Information about the method.</param>
        /// <returns>Collection of parameters.</returns>
        public ParameterModel[] GetParameters(MethodInfo info)
            => info
            .GetParameters()
            .Select(param => new ParameterModel(param))
            .ToArray();

    }
}
