using System;

namespace DotDocs.Core
{
    /// <summary>
    /// The entry-point for .Docs.Core.
    /// </summary>
    public class Docsharker : IDisposable
    {                
        /// <summary>
        /// The main hub for controlling preparing, loading, rendering.
        /// </summary>
        public BuildManager Builder { get; set; }        

        /// <summary>
        /// Initializes a new instance of <see cref="DotDocs"/> loaded with data.
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

        /// <inheritdoc cref="BuildManager.Prepare"/>
        public void Prepare()
            => Builder.Prepare();
        /// <inheritdoc cref="BuildManager.Load"/>
        public void Load()
            => Builder.Load();
        /// <inheritdoc cref="BuildManager.Render"/>
        public void Render()
            => Builder.Render();
    }
}
