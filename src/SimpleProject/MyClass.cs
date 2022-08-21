namespace SimpleProject
{
    public class DerivedClass : MiddleClass
    {
        public override void TestMethod()
        {

        }
    }

    public class MiddleClass : BaseClass
    {

    }

    public class BaseClass
    {
        public virtual void TestMethod()
        {

        }
    }
}