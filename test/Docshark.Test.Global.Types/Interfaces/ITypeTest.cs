using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Test.Global.Types.Interfaces
{
    internal interface ITypeTest
    {
        void Setup();
        void TypeAdded();
        void InDirectParentTypesAdded();
        void DirectTypeParentAdded();
        void InDirectTypeParentsAdded();
    }
}
