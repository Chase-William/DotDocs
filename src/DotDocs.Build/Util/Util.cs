namespace DotDocs.Build.Util
{
    /// <summary>
    /// Contains utility functionalities needed by this project.
    /// </summary>
    public static class Util
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
