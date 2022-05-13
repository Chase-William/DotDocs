using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docsharp.Test.Data.Classes
{
    /// <summary>
    /// A lightweight and narrow vessel used typically in smaller bodies of water.
    /// </summary>
    public class Canoe : Boat
    {
        public override bool TryDock()
        {
            throw new NotImplementedException();
        }

        public override bool TryUndock()
        {
            throw new NotImplementedException();
        }

        public class Builder
        {
            public static Canoe Build()
                => new();
        }
    }
}
