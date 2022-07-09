using LocalProjectDependency;

namespace Charp.Test.Data.Classes
{
    public class ExternalLibDependentClass : ExternalLibraryType
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "myProperty")]
        public int MyProperty { get; set; }
    }
}
