using System;

using Charp.Test.Data.Enumerations;
using Charp.Test.Data.Interfaces;

namespace Charp.Test.Data.Classes
{
    /// <summary>
    /// A boat traditionaly powered via sail used in large bodies of water. 
    /// Usually these boats will feature atleast one engine for emergency senarios.
    /// </summary>
    public sealed class Sailboat : Boat, IPowerable
    {
        public EngineSize Engine { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int EngineCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double SailHeight { get; private set; }
        public override int AbstractProperty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int AbstractNoSetProperty => throw new NotImplementedException();

        public override int AbstractNoGetProperty { set => throw new NotImplementedException(); }

        public override event EventHandler Docked;

        public override bool TryDock()
        {
            throw new NotImplementedException();
        }

        public override bool TryUndock()
        {
            throw new NotImplementedException();            
        }

        /// <summary>
        /// Attemps to deploy the sail to a specified percentage.
        /// </summary>
        /// <param name="percentage">To be deployed to.</param>
        /// <returns>True if deployment was successful, false otherwise.</returns>
        public bool TryDeploySailToPercentage(double percentage) => true;
    }
}
