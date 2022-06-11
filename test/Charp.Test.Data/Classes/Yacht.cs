using Charp.Test.Data.Enumerations;
using Charp.Test.Data.Interfaces;
using System;

namespace Charp.Test.Data.Classes
{
    /// <summary>
    /// A large boat typically used by the weathly or for special occasions in large bodies of water.
    /// </summary>
    public class Yacht : Boat, IPowerable
    {
        public EngineSize Engine { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int EngineCount { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public override event EventHandler Docked;

        public override bool TryDock()
        {
            throw new System.NotImplementedException();
        }

        public override bool TryUndock()
        {
            throw new System.NotImplementedException();
        }

        public class Builder { }
    }
}
