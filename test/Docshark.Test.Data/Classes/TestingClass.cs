using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;

namespace Docshark.Test.Data.Classes
{
    public class TestingClass
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "myProperty")]
        public int MyProperty { get; set; }

        public CommonComments Test { get; set; }

        public Action<string> SingleTypedType { get; set; }
        public Action<string, TestingClass, int, Boat> FourTypeArgumentType { get; set; }
        public Action<string, TestingClass, int, Action<double, string>> FourTypeNestedArgumentType { get; set; }

        public Dictionary<string, TestingClass> DictionaryExample { get; set; }
    }
}
