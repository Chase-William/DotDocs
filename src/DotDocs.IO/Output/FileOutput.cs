using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.IO
{
    public class FileOutput : IOutputable
    {
        public string OutDir { get; private set; }       

        public FileOutput(string outDir)
        {
            // Convert into absolute path to target if needed
            if (!Path.IsPathRooted(outDir))
                OutDir = Path.Combine(Environment.CurrentDirectory, outDir);
            else
                OutDir = outDir;            
        }
        
        public void Prepare()
        {
            // Clean up anything left from last execution
            if (Directory.Exists(OutDir))
                Util.CleanDirectory(OutDir);
            else
                Directory.CreateDirectory(OutDir);
        }

        public bool IsValid()
            => Directory.Exists(OutDir);

        public string GetValue()
            => OutDir;

        public override string ToString()
        {
            return $"{typeof(FileOutput)} | OutDir: {OutDir}";
        }
    }
}
