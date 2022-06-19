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

        protected event EventHandler ProtectedEvent;
        internal event EventHandler InternalEvent;
        internal protected event EventHandler InternalProtectedEvent;
        static internal protected event EventHandler StaticInternalProtectedEvent;

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

        #region Protected Properties
        protected int FullProtectedProperty { get; set; }
        public int GetProtectedProperty { protected get; set; }
        public int SetProtectedProperty { get; protected set; }
        protected int GetProtectedNoSetProperty { get; }
        protected int GetProtectedNoGetProperty { set { } }
        #endregion

        #region Internal Properties
        internal int InternalProperty { get; set; }
        public int InternalGetProperty { internal get; set; }
        public int InternalSetProperty { get; internal set; }
        internal int InternalNoSetProperty { get; }
        internal int InternalNoGetProperty { set { } }
        #endregion

        #region Internal & Protected Properties
        internal protected int InternalProtectedProperty { get; set; }
        public int InternalProtectedGetProperty { internal protected get; set; }
        public int InternalProtectedSetProperty { get; internal protected set; }        
        internal protected int InternalProtectedNoSetProperty { get; }
        internal protected int InternalProtectedNoGetProperty { set { } }
        #endregion

        #region Static & Internal & Protected
        static internal protected int StaticInternalProtectedProperty { get; set; }
        static internal protected int StaticInternalProtectedNoGetProperty { set { } }
        static internal protected int StaticInternalProtectedNoSetProperty { get; }
        #endregion


        public int PrivateSetterProperty { get; private set; }
        public int PrivateGetterProperty { private get; set; }



        protected int ProtectedField;
        internal int InternalField;
        internal protected int InternalProtectedField;
        static internal protected int StaticInternalProtectedField;

        protected void ProtectedMethod() { }
        internal void InternalMethod() { }
        internal protected void InternalProtectedMethod() { }
        static internal protected void StaticInternalProtectedMethod() { }

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

    public class PublicClass
    {
        public class NestedPublicClass { }
        private class NestedPrivateClass { }
        internal class NestedInternalClass { }
        protected class NestedProtectedClass { }
        internal protected class NestedInternalProtectedClass { }
    }

    internal class InternalClass
    {
        public class NestedPublicClass { }
        private class NestedPrivateClass { }
        internal class NestedInternalClass { }
        protected class NestedProtectedClass { }
        internal protected class NestedInternalProtectedClass { }
    }
}
