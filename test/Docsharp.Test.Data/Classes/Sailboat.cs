using System;

using Docsharp.Test.Data.Enumerations;
using Docsharp.Test.Data.Interfaces;

namespace Docsharp.Test.Data.Classes
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
    }
}
