using LocalProjectDependency;
using LoxSmoke.DocXml;

namespace Docshark.Test.Data.Classes
{
    public class ExternalLibDependentClass : ExternalLibraryType
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "myProperty")]
        public int MyProperty { get; set; }

        public CommonComments Test { get; set; }
    }
}
