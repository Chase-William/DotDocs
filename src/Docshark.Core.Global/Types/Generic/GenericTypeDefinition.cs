using Docshark.Core.Models;

namespace Docshark.Core.Global.Types
{
    public class GenericTypeDefinition : Definition
    {
        public string? BaseType { get; set; }
        public string Name { get; set; }
        public string NameWithBase { get; set; }

        public override string GetPrimaryKey()
            => NameWithBase;

        internal static string GetPrimaryKeyMemberName()
            => nameof(NameWithBase);
    }
}
