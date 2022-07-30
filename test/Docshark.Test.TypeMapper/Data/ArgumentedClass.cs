using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Test.TypeMapper.Data
{
    internal class ChildOfArgumentedClass : ArgumentedClass<LeftArgument, RightArgument>
    {

    }

    internal class ArgumentedClass<T, K> : ParentClass
        where T : LeftArgument
        where K : RightArgument
    {
        public T Left { get; set; }
        public K Right { get; set; }
    }

    internal class LeftArgument : LeftArgumentParent
    {

    }

    internal class LeftArgumentParent
    {

    }

    internal class RightArgument
    {

    }  
}
