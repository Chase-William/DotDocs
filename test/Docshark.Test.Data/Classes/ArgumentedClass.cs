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

    public class ArgumentedBaseClass<T, K> : ArgumentedSuperBaseClass<T, K>
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

    public class ArgumentedSuperBaseClass<T, K> : ArgumentedSuperBaseClass1
        where T : LeftBaseArgument
        where K : RightBaseArgument<LeftArgumentLeftArgument, RightArgumentRightArgument>
    { }

    public class ArgumentedSuperBaseClass1 : ArgumentedSuperBaseClass2 { }
    public class ArgumentedSuperBaseClass2 : ArgumentedSuperBaseClass3 { }
    public class ArgumentedSuperBaseClass3 : ArgumentedSuperBaseClass4 { }
    public class ArgumentedSuperBaseClass4 : ArgumentedSuperBaseClass5 { }
    public class ArgumentedSuperBaseClass5 : ArgumentedSuperBaseClass6 { }
    public class ArgumentedSuperBaseClass6 : ArgumentedSuperBaseClass7 { }
    public class ArgumentedSuperBaseClass7 : ArgumentedSuperBaseClass8 { }
    public class ArgumentedSuperBaseClass8 { }
}
