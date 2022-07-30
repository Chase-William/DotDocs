using System;
using System.Collections.Generic;
using System.Text;

namespace Docshark.Test.Data.Classes
{
    public class FromArgumentedClasses : ArgumentedBaseClass<LeftArgument, RightBaseArgument<LeftArgumentLeftArgument, RightArgumentRightArgument>>
    {

    }

    public class LeftArgument : LeftBaseArgument
    {

    }  

    public class ArgumentedBaseClass<T, K> : ArgumentedSuperBaseClass
        where T : LeftBaseArgument
        where K : RightBaseArgument<LeftArgumentLeftArgument, RightArgumentRightArgument>
    {

    }

    public class LeftBaseArgument
    {

    }

    public class RightBaseArgument<T, K>
        where T : LeftArgumentLeftArgument
        where K : RightArgumentRightArgument
    {

    }

    public class RightArgumentRightArgument
    {

    }

    public class LeftArgumentLeftArgument
    {

    }

    public class ArgumentedSuperBaseClass
    {

    }
}
