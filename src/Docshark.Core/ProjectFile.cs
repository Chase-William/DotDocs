using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Docshark.Core
{
    internal class ProjectFile
    {
        public string TargetFramework { get; set; }
        public string AssemblyName { get; set; }

        private ProjectFile() { }

        bool produceReferenceAssembly;
        bool generateDocumentationFile;
        string projectFile;

        public static ProjectFile ConfigureFrom(string projectFile)
        {
            var doc = XDocument.Parse(File.ReadAllText(projectFile));
            var proj = new ProjectFile
            {
                projectFile = projectFile
            };

            var propertyGroup = doc.XPathSelectElement("//PropertyGroup").Descendants();

            foreach (var item in propertyGroup)
            {
                switch (item.Name.LocalName)
                {
                    case "TargetFramework":
                        proj.TargetFramework = item.Value;
                        break;
                    case "ProduceReferenceAssembly":
                        proj.produceReferenceAssembly = bool.Parse(item.Value);
                        break;
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

            proj.OverwriteIfNeeded(doc);

            return proj;
        }

        /// <summary>
        /// Overwrites an existing .csproj with additional required settings if needed.
        /// </summary>
        void OverwriteIfNeeded(XDocument doc)
        {
            bool needsOverwrite = false;

            var props = doc.XPathSelectElement("//PropertyGroup");
            if (!produceReferenceAssembly)
            {
                props.Add(new XElement("ProduceReferenceAssembly")
                {
                    Value = "True"
                });
                needsOverwrite = true;
            }

            if (!generateDocumentationFile)
            {
                props.Add(new XElement("GenerateDocumentationFile")
                {
                    Value = "True"
                });
                needsOverwrite = true;
            }

            if (needsOverwrite)
            {
                using (var stream = File.Open(projectFile, FileMode.Create))
                {
                    doc.Save(stream);
                }
            }

            produceReferenceAssembly = true;
            generateDocumentationFile = true;
        }
    }
}
