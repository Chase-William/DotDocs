﻿using System;
using System.Reflection;

namespace Docshark.Core.Models.Codebase
{
    /// <summary>
    /// Represents a type that has functional based documentation like parameters and return type.
    /// </summary>
    public interface IFunctional
    {
        /// <summary>
        /// Return type of the function signature.
        /// </summary>
        public TypeKey ReturnType { get; }

        /// <summary>
        /// Parameters used in the function signature.
        /// </summary>
        public TypeKeyParameter[] Parameters { get; }

        /// <summary>
        /// Gets all the parameter information and organizes it before returning it.
        /// </summary>
        /// <param name="info">Information about the method.</param>
        /// <returns>Collection of parameters.</returns>
        public TypeKeyParameter[] GetParameters(MethodInfo info)
        {
            var _params = info.GetParameters();
            if (_params.Length == 0)
                return Array.Empty<TypeKeyParameter>();
            var tempParam = new TypeKeyParameter[_params.Length];
            for (int i = 0; i < _params.Length; i++)
                tempParam[i] = TypeKeyParameter.From(_params[i]);
            return tempParam;
        }
    }
}
