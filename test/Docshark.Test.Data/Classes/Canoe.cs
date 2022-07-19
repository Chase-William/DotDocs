using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Test.Data.Classes
{
    /// <summary>
    /// A lightweight and narrow vessel used typically in smaller bodies of water.
    /// </summary>
    public class Canoe : Boat
    {
        public override int AbstractProperty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int AbstractNoSetProperty => throw new NotImplementedException();

        public override int AbstractNoGetProperty { set => throw new NotImplementedException(); }

        public override event EventHandler Docked;

        public override bool TryDock()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">Testing 123</exception>
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
