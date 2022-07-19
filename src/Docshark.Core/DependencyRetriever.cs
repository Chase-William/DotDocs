using System.Xml.Linq;
using System.IO;
using System.Xml.XPath;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Docshark.Core
{
    /**
     * Projects referenced via PackageReference in a .csproj always use the global-packages folder.
     * 
     * src:
     * https://docs.microsoft.com/en-us/nuget/consume-packages/managing-the-global-packages-and-cache-folders
     * 
     * 
     */
    internal static class DependencyRetriever
    {
        public static string[] FindAll(string csProjPath, string[] runtimeDirAssemblies)
        {
            // Read .csproj
            var doc = XDocument.Parse(File.ReadAllText(csProjPath));


            var libraries = GetNugetPackages(doc);
            libraries.AddRange(GetLocalProjects(doc, csProjPath.Substring(0, csProjPath.LastIndexOf('\\'))));
            libraries.AddRange(runtimeDirAssemblies);

            return libraries.ToArray();
        }

        /// <summary>
        /// Gets all the paths to the local projects referenced in this project.
        /// In the case of multiple instances of the .dll in different dirs, the most
        /// recently modified is used.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="csProjPath">Base main csproj all other local csprojs are relative to.</param>
        /// <returns></returns>
        static List<string> GetLocalProjects(XDocument doc, string csProjPath)
        {
            // Get locally referenced packages
            var projectReferences = doc.XPathSelectElements("//ProjectReference")
                .Select(pr => new ProjectReference
                {
                    Include = pr.Attribute("Include").Value
                });

            var localDlls = new List<string>();

            foreach (var project in projectReferences)
            {
                int dirEndPos = project.Include.LastIndexOf('\\');
                int beforeExtension = project.Include.LastIndexOf('.');
                // Get paths to all the dlls with the same name
                var dlls = new List<(string, DateTime)>();
                
                FindAllDlls(
                    Path.Join(csProjPath, project.Include.Substring(0, dirEndPos), "bin"),
                    // Get the name of the csproj and append ".dll" as the dll will have the same name
                    project.Include.Substring(
                        dirEndPos + 1, // omit slash itself
                        beforeExtension - dirEndPos)
                        + "dll",
                    dlls
                );

                // Compare which was modified last
                var lastModifiedDll = dlls.Aggregate((firstDll, secondDll) => firstDll.Item2 > secondDll.Item2 ? firstDll : secondDll).Item1;
                localDlls.Add(lastModifiedDll);
            }

            return localDlls;
        }

        static void FindAllDlls(string path, string targetDllName, List<(string, DateTime)> dlls)
        {
            // var currentDir = path.Aggregate((str1, str2) => Path.Join(str1, str2));
            var dirs = Directory.GetDirectories(path);
            
            foreach (var dir in dirs)
            {
                //path.Push(dir);
                FindAllDlls(dir, targetDllName, dlls);
                //path.Pop();
            }

            string[] files = Directory.GetFiles(path);            

            foreach (var fileName in files)           
                if (fileName.EndsWith(targetDllName))
                {
                    dlls.Add((fileName, File.GetLastWriteTime(fileName)));
                    break;
                }            
        }

        /// <summary>
        /// Gets all the paths to nuget packages referenced in this project.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        static List<string> GetNugetPackages(XDocument doc)
        {
            // Get nuget packages
            var packageReferences = doc.XPathSelectElements("//PackageReference")
                .Select(pr => new PackageReference
                {
                    Include = pr.Attribute("Include").Value,
                    Version = new Version(pr.Attribute("Version").Value)
                });

            // Nuget base dir
            string nugetDir = Path.Join(Environment.ExpandEnvironmentVariables("%userprofile%"), ".nuget\\packages");

            var nugetPackagePaths = new List<string>();

            foreach (var package in packageReferences)
            {
                string temp = Path.Join(nugetDir, package.Include.ToLower(), package.Version.ToString(), "lib");
                nugetPackagePaths.Add(Path.Join(Directory.GetDirectories(temp).First(), $"{package.Include}.dll"));
            }

            return nugetPackagePaths;
        }

        public class PackageReference : ProjectReference
        {
            public Version Version { get; set; }
        }

        public class ProjectReference
        {
            public string Include { get; set; }
        }
    }
}
