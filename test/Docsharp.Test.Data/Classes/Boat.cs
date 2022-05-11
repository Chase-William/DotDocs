using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docsharp.Test.Data.Classes
{
    /// <summary>
    /// Represents the core common aspects of a boat.
    /// </summary>
    public abstract class Boat
    {
        /// <summary>
        /// Distance from stern to bow of a boat.
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Mass of the boat in tons.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Attempts to dock the ship.
        /// </summary>
        /// <returns></returns>
        public abstract bool TryDock();

        /// <summary>
        /// Attempts to undock the ship.
        /// </summary>
        /// <returns></returns>
        public abstract bool TryUndock();
    }
}
