using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Core.Loader
{
    /// <summary>
    /// Read in HEAD file and follow the provided path to get the commit hash of head.
    /// </summary>
    public static class GitService
    {
        /// <summary>
        /// Downloads a repository and returns the path to the repository.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string DownloadRepository(string url)
        {
            // CODE FOR DOWNLOADING AND BUILDNG
            string directory = AppContext.BaseDirectory; // directory of process execution
            string downloadRepoLocation = Path.Combine(directory, "downloads");
            if (!Directory.Exists(downloadRepoLocation))
                Directory.CreateDirectory(downloadRepoLocation);

            using PowerShell powershell = PowerShell.Create();
            // this changes from the user folder that PowerShell starts up with to your git repository
            powershell.AddScript($"cd {downloadRepoLocation}");
            powershell.AddScript(@"git clone https://github.com/Chase-William/.Docs.Core.git");
            //powershell.AddScript("cd.. / .. /.Docs.Core");
            //powershell.AddScript("dotnet build");
            powershell.Invoke(); // Run powershell            

            var folder = url.Split("/").Last();
            if (folder.Contains(".git"))
                folder = folder[..4];
            
            return Path.Combine(downloadRepoLocation, folder);
        }

        /// <summary>
        /// Retrieves the current hash for the HEAD commit of the downloaded repository.
        /// </summary>
        /// <param name="repoDir">Base directory of the repo.</param>
        /// <returns>Commit HEAD Hash</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string GetGitHeadHash(string repoDir)
        {
            string gitHeadFile = Path.Combine(repoDir, @".git\HEAD");
            if (!File.Exists(gitHeadFile))
                throw new FileNotFoundException($"File 'HEAD' was not found at: {gitHeadFile}. Has the repository been downloaded using 'git clone <repo-url>' yet?");

            string commitHashFilePath = File.ReadAllText(gitHeadFile);
            // 'ref: ' <- skip these characters and get file dir that follows
            commitHashFilePath = Path.Combine(repoDir, ".git", commitHashFilePath[5..]
                .Replace("\n", "")
                .Replace("/", "\\")
                .Trim());

            if (!File.Exists(commitHashFilePath))
                throw new FileNotFoundException($"The file containing the current HEAD file hash was not found at: {commitHashFilePath}");

            return File.ReadAllText(commitHashFilePath)
                .Replace("\n", "")
                .Trim();
        }
    }
}
