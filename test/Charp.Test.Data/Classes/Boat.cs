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
        /// A simple public event.
        /// </summary>
        public event EventHandler PublicEvent;
        /// <summary>
        /// A simple private event.
        /// </summary>
        event EventHandler PrivateEvent;
        /// <summary>
        /// Notifies subscribers the <see cref="Point"/> has docked.
        /// </summary>
        public abstract event EventHandler Docked;
        /// <summary>
        /// Notifies subscribers the <see cref="Boat"/> has undocked.
        /// </summary>
        public virtual event EventHandler UnDocked
        {
            add
            {

            }

            remove
            {

            }
        }

        public virtual event EventHandler VirtualEvent;

        public static event EventHandler StaticEvent;

        /// <summary>
        /// Distance from stern to bow of a boat.
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Mass of the boat in tons.
        /// </summary>
        public double Weight { get; set; }

        public int NoSetterProperty { get; }        

        public int NoGetterProperty
        {
            set
            {

            }
        }

        public int PrivateSetterProperty { get; private set; }
        public int PrivateGetterProperty { private get; set; }

        /// <summary>
        /// My field does what a field does.
        /// </summary>
        public double MyField;

        /// <summary>
        /// Does what a static field would do.
        /// </summary>
        public static int MyStaticField;

        /// <summary>
        /// A constant field.
        /// </summary>
        public const string MyConstantField = "I am constant :P";

        /// <summary>
        /// A readonly field.
        /// </summary>
        public readonly string MyReadonlyField 
            = "I have an initial value or can be set in ctor.. thats all..";

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

        public static void ExampleStaticMethod(int a, Sailboat b)
        {

        }
    }
}
