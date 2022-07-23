using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Docshark.Core
{
    public class ProjectFile
    {
        public string TargetFramework { get; set; }
        public string AssemblyName { get; set; }

        private ProjectFile() { }

        XDocument docFile;
        // bool produceReferenceAssembly;
        bool generateDocumentationFile;
        string projectFile;

        /**
         * msbuild ProjDepResolver.Runner.csproj -fl1 -fl2 -flp1:logfile=errors.log;errorsonly -flp2:logfile=details.log;details
         * dotnet msbuild ./ProjDepResolver.Runner/ProjDepResolver.Runner.csproj -fl1 -fl2 -flp1:logfile=errors.log;errorsonly -flp2:logfile=details.log;
         */

        public static ProjectFile From(string csProjFile)
        {            
            var proj = new ProjectFile
            {
                docFile = XDocument.Parse(File.ReadAllText(csProjFile)),
                projectFile = csProjFile
            };

            var propertyGroup = proj.docFile.XPathSelectElement("//PropertyGroup").Descendants();

            foreach (var item in propertyGroup)
            {
                switch (item.Name.LocalName)
                {
                    case "TargetFramework":
                        proj.TargetFramework = item.Value;
                        break;
                    //case "ProduceReferenceAssembly":
                    //    proj.produceReferenceAssembly = bool.Parse(item.Value);
                    //    break;
                    case "AssemblyName":
                        proj.AssemblyName = item.Value;
                        break;
                    case "GenerateDocumentationFile":
                        proj.generateDocumentationFile = bool.Parse(item.Value);
                        break;
                    default:
                        break;
                }
            }            

            return proj;
        }        

        public bool ApplyDocsharkConfiguration()
        {
            var performSave = false;

            var props = docFile.XPathSelectElement("//PropertyGroup");
            //if (!produceReferenceAssembly)
            //{
            //    props.Add(new XElement("ProduceReferenceAssembly")
            //    {
            //        Value = "True"
            //    });
            //    performSave = true;
            //}

            if (!generateDocumentationFile)
            {
                props.Add(new XElement("GenerateDocumentationFile")
                {
                    Value = "True"
                });
                performSave = true;
            }

            return performSave;
        }

        public void Save()
        {
            using var stream = File.Open(projectFile, FileMode.Create);
            docFile.Save(stream);
        }

        public bool TryBuildProject(string csProjPath, out string targetAssemblyPath, out string[] depAssemblyPaths)
        {
            try
            {
                var cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.StartInfo.Arguments = $"/C dotnet msbuild {csProjPath} /bl";
                cmd.Start();
                // Wait for files to finish being written & process close
                cmd.WaitForExit();

                var build = BinaryLog.ReadBuild("msbuild.binlog");

                var project = build
                    .FindChild<Project>();

                { // Locate paths to all dependency assemblies
                    var target = project
                    .FindFirstChild<Target>(c => c.Name == "ResolveReferences");
                    var refFolder = target
                        .FindChild<Folder>("TargetOutputs");
                    var references = refFolder.Children;
                    depAssemblyPaths = references.Select(item => (item as Item).Text).ToArray();
                }

                { // Locate root assembly
                    var target = project.FindChild<Target>("GetTargetPath");
                    var outputs = target.FindChild<Folder>("TargetOutputs");
                    targetAssemblyPath = ((Item)outputs.Children.First()).ShortenedText;
                }             
            }
            catch
            {
                throw;
            }


            return true;
        }
    }
}
