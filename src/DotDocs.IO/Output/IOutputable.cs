using DotDocs.IO.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.IO
{
    public interface IOutputable
    {
        public IRouterable Router { get;}

        public void Clean();

        public void Write(Type type, in StringBuilder builder);
    }
}
