using System;
using System.Reflection;

namespace Charp.Core.Models
{
    /// <summary>
    /// Represents a type that has functional based documentation like parameters and return type.
    /// </summary>
    public interface IFunctional
    {
        /// <summary>
        /// Return type of the function signature.
        /// </summary>
        public string ReturnType { get; }

        /// <summary>
        /// Parameters used in the function signature.
        /// </summary>
        public Parameter[] Parameters { get; }

        /// <summary>
        /// Gets all the parameter information and organizes it before returning it.
        /// </summary>
        /// <param name="info">Information about the method.</param>
        /// <returns>Collection of parameters.</returns>
        public Parameter[] GetParameters(MethodInfo info)
        {
            var _params = info.GetParameters();
            if (_params.Length == 0)
                return Array.Empty<Parameter>();
            var tempParam = new Parameter[_params.Length];
            for (int i = 0; i < _params.Length; i++)
                tempParam[i] = new Parameter
                {
                    Name = _params[i].Name,
                    Type = _params[i].ParameterType.ToString()
                };
            return tempParam;
        }
    }
}
