using DotDocs.IO.Routing;
using System.Text;

namespace DotDocs.IO
{
    /// <summary>
    /// Perform output to the file system.
    /// </summary>
    public class TextFileOutput : IOutputable
    {
        public string OutDirBase { get; private set; }

        public IRouterable Router { get; private set; }

        public string FileEx { get; private set; }

        public TextFileOutput(string outDir, IRouterable router, string fileEx)
        {
            Router = router;
            FileEx = fileEx;

            // Convert into absolute path to target if needed
            if (!Path.IsPathRooted(outDir))
                OutDirBase = Path.Combine(Environment.CurrentDirectory, outDir);
            else
                OutDirBase = outDir;            
        }
        
        /// <summary>
        /// Prepares the expected out dir for writing new material.
        /// </summary>
        public void Clean()
        {
            // Clean up anything left from last execution
            if (Directory.Exists(OutDirBase))
                Util.CleanDirectory(OutDirBase);
            else
                Directory.CreateDirectory(OutDirBase);
        }

        /// <summary>
        /// Checks
        /// </summary>
        /// <returns></returns>
        //public bool IsValid()
        //    => Directory.Exists(OutDirBase);

        //public string GetValue()
        //    => OutDirBase;

        //public string GetValue(Type type)
        //    => Path.Combine(OutDirBase, Router.GetDir(type), Router.GetFileName(type));

        public override string ToString()
        {
            return $"{typeof(TextFileOutput)} | OutDir: {OutDirBase}";
        }

        public void Write(Type type, in StringBuilder builder)
        {
            try
            {
                string folderPath = Path.Combine(OutDirBase, Router.GetDir(type));
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                using var fstream = File.CreateText(Path.Combine(folderPath, Router.GetFileName(type)) + FileEx);
                fstream.Write(builder);
            }
            catch
            {
                throw;
            }                     
        }
    }
}
