using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Source.One.Supporting;

namespace Test.Source.One
{
    public class MyFields
    {
        public int MyField01;
        public MyFields MyField02;
        public MyGenericClassRef<int, string> MyField03;
        public (int, string) MyField04;
        public (int index, string substr) MyField05;
    }
}
