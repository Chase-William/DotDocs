using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.IO.Routing
{
    public interface IRouterable
    {
        public string GetRoute(Type from, Type to);
        public string GetName(Type type);
        public string GetLocation(Type type);
    }
}
