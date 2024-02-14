using Microsoft.Build.Construction;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace DotDocs.Build
{
    public static class ProjectInSolutionEx
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static void EnableDocGen(this ProjectInSolution proj)
        {
            Logger.Trace("Enabling documentation generation for project file: {projectfile}", proj.AbsolutePath);

            var xDoc = XDocument.Parse(File.ReadAllText(proj.AbsolutePath));

            // Enable documenation generation on current project
            var propertyGroup = xDoc.XPathSelectElement("//PropertyGroup");
            var docGenProp = propertyGroup.Descendants().SingleOrDefault(prop => prop.Name.LocalName == "GenerateDocumentationFile");
            // Add to .csproj file if it doesn't exist (never was enabled or disabled)
            if (docGenProp == null)
                propertyGroup.Add(new XElement("GenerateDocumentationFile")
                {
                    Value = "True"
                });
            // Update to true if set to false
            else if (bool.Parse(docGenProp.Value.ToLower()) == false)
                docGenProp.Value = "True";

            using var writer = new StreamWriter(proj.AbsolutePath);
            xDoc.Save(writer);
            // Close writer before processing child nodes
        }

        public static Microsoft.Build.Logging.StructuredLogger.Build Build(
            this ProjectInSolution project)
        {
            Logger.Trace("Building the solution.");

            var projectDir = project.AbsolutePath[..project.AbsolutePath.LastIndexOf('\\')];

            var binLogPath = Path.Combine(projectDir, "msbuild.binlog");
            if (File.Exists(binLogPath))
                File.Delete(binLogPath);

            var cmd = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    Arguments = $"/C dotnet build {project.ProjectName + ".csproj"} /bl",
                    WorkingDirectory = projectDir
                }
            };

            try
            {
                Logger.Debug("Launching a {process} process.", cmd.StartInfo.FileName);
                cmd.Start();

                // Wait for files to finish being written & process close
                cmd.WaitForExit();
                return BinaryLog.ReadBuild(binLogPath);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                throw;
            }
        }
    }
}
