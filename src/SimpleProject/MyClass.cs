namespace SimpleProject
{    
    public class AnotherClass<T1> : MyClass<T1>
        where T1 : MyNextClass
    {

    }

    public class MySemiDoneClass<T1> : MyClass<MyOtherClass>
    {

    }

    public class MyClass<T1>
        where T1 : MyOtherClass
    {

    }

    public class MyNextClass : MyOtherClass
    {

    }

    public class MyOtherClass
    {

    }
}