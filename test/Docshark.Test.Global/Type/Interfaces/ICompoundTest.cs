using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Test.Global.Types.Interfaces
{
    internal interface ICompoundTest
    {
        void DirectTypeArgumentsAddedToDictionary();
        void TypeArgumentsAddedToArgumentList();
        void InDirectTypeArgumentsAddedToDictionary();
        void TypeArgumentNotDuplicated();
    }
}
