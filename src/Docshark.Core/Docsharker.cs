using System;
using Docshark.Core.Exceptions;
using Docshark.Core.Tree;

namespace Docshark.Core
{
    /// <summary>
    /// The main hub for Charp.Core.
    /// </summary>
    public class Docsharker : IDisposable
    {                
        public BuildManager Builder { get; set; }        

        /// <summary>
        /// Initializes a new instance of <see cref="Docshark"/> loaded with data.
        /// </summary>
        /// <param name="csProjFile">csProject used for locating dependencies and dll/xml if needed.</param>
        /// <param name="outputPath">Location for JSON output.</param>
        public Docsharker(string csProjFile, string outputPath)
            => Builder = new BuildManager(csProjFile, outputPath);                           

        /// <summary>
        /// Cleanup unmanaged resources linked with <see cref="Builder"/>.
        /// </summary>
        public void Dispose()
        {
            Builder?.Dispose();
            Builder = null;
        }

        public void Prepare()
            => Builder.Prepare();

        public void Load()
            => Builder.Load();

        public void Make()
            => Builder.Make();
    }
}
