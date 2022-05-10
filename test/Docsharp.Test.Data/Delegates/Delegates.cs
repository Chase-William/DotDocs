using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docsharp.Test.Data.Delegates
{
    /// <summary>
    /// Do some math delegate here.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public delegate int DoSomeMathWillYou(int x, int y);

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
