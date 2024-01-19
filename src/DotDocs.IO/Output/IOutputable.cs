using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.IO
{
    public interface IOutputable
    {
        public bool IsValid();

        public void Prepare();

        public string GetValue();
    }
}
