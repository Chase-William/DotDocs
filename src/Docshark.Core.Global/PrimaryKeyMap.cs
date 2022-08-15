using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Core.Global
{
    internal struct PrimaryKeyMap
    {
        public string DefinitionTypeName { get; set; }
        public string PrimaryKeyMemberName { get; set; }
        public bool IsComposite { get; set; }
    }
}
