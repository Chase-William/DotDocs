namespace DotDocs
{
    /// <summary>
    /// The entry-point for .Docs.Core.
    /// </summary>
    public class DotDocs
    {
        public static Builder New(string url)
            => new(url);        
    }
}
