using Docsharp.Test.Data.Structs;

namespace Docsharp.Test.Data.Delegates
{
    /// <summary>
    /// Do some math delegate here.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public delegate void DoSomeMathWillYou(double x, Rectangle y);

    /// <summary>
    /// Class for nested delegate.
    /// </summary>
    public class NestedInClassDelegate
    {
        /// <summary>
        /// Do some nested stuff delegate here.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public delegate int DoSomeNestedStuff(int x, int y);

        /// <summary>
        /// Nested Test Class.
        /// </summary>
        public class Test
        {

        }
    }
}
