using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models
{
    public class AssemblyReflectInfo : IDisposable
    {
        public string AbsoluteDocPath { get; init; }
        public Assembly Binary { get; init; }

        MetadataLoadContext mlc;

        public AssemblyReflectInfo(
            string absoluteDocPath,
            string binaryPath,
            MetadataLoadContext mlc)
        {
            AbsoluteDocPath = absoluteDocPath;
            this.mlc = mlc;
            Binary = mlc.LoadFromAssemblyPath(binaryPath);
        }

        public void Dispose()
        {
            mlc.Dispose();
        }
    }
}
