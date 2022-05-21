using Charp.Test.Data.Enumerations;
using Charp.Test.Data.Interfaces;

namespace Charp.Test.Data.Classes
{
    /// <summary>
    /// The most common type of boat used in medium to large size bodies of water.
    /// </summary>
    public class Runabout : Boat, IPowerable
    {
        public EngineSize Engine { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int EngineCount { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public override bool TryDock()
        {
            throw new System.NotImplementedException();
        }

        public override bool TryUndock()
        {
            throw new System.NotImplementedException();
        }
    }
}
