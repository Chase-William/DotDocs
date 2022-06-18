using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using LoxSmoke.DocXml;

namespace Charp.Core.Models.Members
{
    internal interface IAccessible
    {
        public bool IsInternal { get; }
        public bool IsPublic { get; }
    }

    internal interface IMemberable : IAccessible
    {
        public bool IsProtected { get; }
    }
}
