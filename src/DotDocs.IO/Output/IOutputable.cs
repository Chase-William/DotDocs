using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.IO
{
    public interface IOutputable
    {        
        public void Clean();

        public void Write(Type type, in StringBuilder builder);
    }
}
