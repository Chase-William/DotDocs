using Docsharp.Test.Data.Enumerations;
using Docsharp.Test.Data.Interfaces;

namespace Docsharp.Test.Data.Classes
{
    /// <summary>
    /// A large boat typically used by the weathly or for special occasions in large bodies of water.
    /// </summary>
    public class Yacht : Boat, IPowerable
    {
        public EngineSize Engine { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int EngineCount { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
