using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Source.One.Supporting;

namespace Test.Source.One
{
    public class MyEvents
    {
        public event Action MyEvent01;
        public event Action<int> MyEvent02;
        public event Action<int, string> MyEvent03;
        public event EventHandler MyEvent04;
        public event EventHandler<MyGenericClassRef<int, string>> MyEvent05;
    }
}
