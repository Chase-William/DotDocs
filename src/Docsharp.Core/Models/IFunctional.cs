using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Docsharp.Core.Models
{
    public interface IFunctional
    {
        public string ReturnType { get; }
        public Parameter[] Parameters { get; }

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
