using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace DotDocs.Core.Loader
{    
    /// <summary>
    /// Contains utility functionalities needed by this project.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Deletes all elements within a directory if it exists and ensures the directory given exist afterwards.
        /// </summary>
        /// <param name="pathToClean">Path to be cleaned.</param>
        public static void CleanDirectory(string pathToClean)
        {
            // First clean anything inside the dir
            if (Directory.Exists(pathToClean))
                Directory.Delete(pathToClean, true);
            Directory.CreateDirectory(pathToClean);
        }        
    }        
}
