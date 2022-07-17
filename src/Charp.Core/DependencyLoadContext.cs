using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Charp.Core
{
    public class DependencyLoadContext
    {
        public string[] AssemblyReferences { get; set; }

        private DependencyLoadContext() { }

        public static DependencyLoadContext From(string rootCSProjFilePath)
        {
            var depLoadCtx = new DependencyLoadContext();
            depLoadCtx.ConfigureProjectFile(rootCSProjFilePath);
            depLoadCtx.TryBuildProject(rootCSProjFilePath);
            return depLoadCtx;
        }

        void TryBuildProject(string projectFile)
        {
            var cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.Arguments = $"/C dotnet msbuild {projectFile} /bl";
            cmd.Start();
            // Wait for files to finish being written & process close
            cmd.WaitForExit();

            var build = BinaryLog.ReadBuild("msbuild.binlog");

            var project = build
                .FindChild<Project>();
            var target = project
                .FindFirstChild<Target>(c => c.Name == "ResolveReferences");
            var refFolder = target
                .FindChild<Folder>("TargetOutputs");
            var references = refFolder.Children;

            AssemblyReferences = references.Select(item => (item as Item).Text).ToArray();
        }

        /**
         * 
         * msbuild ProjDepResolver.Runner.csproj -fl1 -fl2 -flp1:logfile=errors.log;errorsonly -flp2:logfile=details.log;details
         * dotnet msbuild ./ProjDepResolver.Runner/ProjDepResolver.Runner.csproj -fl1 -fl2 -flp1:logfile=errors.log;errorsonly -flp2:logfile=details.log;
         */

        void ConfigureProjectFile(string projectFile)
        {
            if (!File.Exists(projectFile))
                throw new FileNotFoundException($"Could not find {projectFile}.");
            ProjectFile.ConfigureFrom(projectFile);
        }
    }
}
