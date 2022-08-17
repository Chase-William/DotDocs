namespace SimpleProject
{
    public class TopLevelClass : GenericParameterizedClass<ArgumentClass, ArgumentClass>
    {

    }

    //public class TopLevelGenericishClass<T1> : GenericParameterizedClass<ArgumentClass>
    //{

    //}

    public class GenericParameterizedClass<T1, T2>
        where T2 : ArgumentClass
    {

    }

    public class SecondGenericParameterizedClass<T1, T2>
       where T2 : ArgumentClass
    {

    }

    public class ArgumentClass
    {

    }
}