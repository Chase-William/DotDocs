using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Source.One
{
    public class MyMethods
    {
        public void MyMethod01() => throw new NotImplementedException();
        public int MyMethod02() => throw new NotImplementedException();
        public void MyMethod03(int index) => throw new NotImplementedException();
        public int MyMethod04(int index) => throw new NotImplementedException();
        public T MyMethod05<T>(T obj) => throw new NotImplementedException();
        public (T, int) MyMethod06<T>(T obj, int index) => throw new NotImplementedException();
    }
}
