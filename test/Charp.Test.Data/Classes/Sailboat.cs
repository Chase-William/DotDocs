using System;

using Charp.Test.Data.Enumerations;
using Charp.Test.Data.Interfaces;

namespace Charp.Test.Data.Classes
{
    /// <summary>
    /// A boat traditionaly powered via sail used in large bodies of water. 
    /// Usually these boats will feature atleast one engine for emergency senarios.
    /// </summary>
    public class Sailboat : Boat, IPowerable
    {
        public EngineSize Engine { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int EngineCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double SailHeight { get; private set; }

        public override bool TryDock()
        {
            throw new NotImplementedException();
        }

        public override bool TryUndock()
        {
            throw new NotImplementedException();
        }
    }
}
