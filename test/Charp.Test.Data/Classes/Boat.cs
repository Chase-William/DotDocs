using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Charp.Test.Data.Structs;

namespace Charp.Test.Data.Classes
{
    /// <remarks>
    /// Blah Blah Blah
    /// <para>
    /// From para
    /// </para>
    /// </remarks>
    /// <summary>
    /// Represents the core common aspects of a boat.
    /// <para>
    /// Hello
    /// <para>
    /// Inside Nested Para
    /// </para>
    /// </para>
    /// </summary>    
    public abstract class Boat
    {
        /// <summary>
        /// Notifies subscribers the <see cref="Point"/> has docked.
        /// </summary>
        public event EventHandler Docked;
        /// <summary>
        /// Notifies subscribers the <see cref="Boat"/> has undocked.
        /// </summary>
        public event EventHandler UnDocked
        {
            add
            {

            }

            remove
            {

            }
        }

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
