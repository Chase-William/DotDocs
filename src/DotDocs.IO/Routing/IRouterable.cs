using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.IO.Routing
{
    public interface IRouterable
    {
        public string GetFileName(Type type);
        public string GetDir(Type type);
    }
}
