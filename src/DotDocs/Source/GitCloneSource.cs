using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Source
{
    internal class GitCloneSource : ISourceable
    {
        public string Src { get; init; }

        public GitCloneSource(string src)
            => Src = src;

        public ISourceable Prepare()
        {
            // Setup pathing
            string directory = AppContext.BaseDirectory; // directory of process execution
            string downloadRepoLocation = Path.Combine(directory, "downloads");
            // Ensure download location exists
            if (!Directory.Exists(downloadRepoLocation))
                Directory.CreateDirectory(downloadRepoLocation);

            using PowerShell powershell = PowerShell.Create();
            // this changes from the user folder that PowerShell starts up with to your git repository
            powershell.AddScript($"cd {downloadRepoLocation}");
            powershell.AddScript($"git clone {Src}");
            //powershell.AddScript("cd.. / .. /.Docs.Core");
            //powershell.AddScript("dotnet build");
            powershell.Invoke(); // Run powershell            

            var folder = Src.Split("/").Last();
            if (folder.Contains(".git"))
                folder = folder[..4];

            // Return a new local source for the downloaded repo
            return new LocalSource(Path.Combine(downloadRepoLocation, folder));
        }
    }
}
