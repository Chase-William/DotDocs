using DotDocs.IO.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.IO
{
    /// <summary>
    /// An interface that is responsible for writing <see cref="StringBuilder"/> content to file using an <see cref="IRouterable"/>.
    /// </summary>
    public interface IOutputable
    {
        public IRouterable Router { get;}

        public void Write(Type type, in StringBuilder builder);
    }
}
