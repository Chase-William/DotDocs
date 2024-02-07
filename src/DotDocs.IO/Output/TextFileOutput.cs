using DotDocs.IO.Routing;
using System.Text;

namespace DotDocs.IO
{
    /// <summary>
    /// The <c>TextFileOutput</c> class performs file output operations.
    /// </summary>
    public class TextFileOutput : IOutputable
    {
        /// <summary>
        /// The output directory all routing extends from.
        /// </summary>
        public string OutDirBase { get; private set; }

        /// <summary>
        /// The router to be used determining the naming/location of files relative to the provided <see cref="OutDirBase"/>.
        /// </summary>
        public IRouterable Router { get; private set; }

        /// <summary>
        /// The file extension.
        /// </summary>
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
        /// Writes contents of the <see cref="StringBuilder"/> to file.
        /// </summary>
        /// <param name="type">The type the file represents.</param>
        /// <param name="builder">A reference to the builder containing the text.</param>
        public void Write(Type type, in StringBuilder builder)
        {            
            string folderPath = Path.Combine(OutDirBase, Router.GetLocation(type));
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            using var fstream = File.CreateText(Path.Combine(folderPath, Router.GetName(type)) + FileEx);
            fstream.Write(builder);                               
        }
    }
}
