using Docshark.Core.Exceptions;
using Docshark.Core.Loaders;
using Docshark.Core.Models;
using Docshark.Core.Tree;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Core
{
    public class Builder : IDisposable
    {
        /// <summary>
        /// A tree that organizes all types.
        /// </summary>
        public ModelTree Models { get; private set; }

        /// <summary>
        /// Destination directory for JSON.
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// Path to .csproj.
        /// </summary>
        public string ProjectPath { get; set; }

        /// <summary>
        /// Contains all reflection based metadata.
        /// </summary>
        MetadataLoader metadata;

        string targetAsmPath;
        string[] depAsmPaths;

        public Builder(string outputPath, string csProjFile)
        {
            OutputPath = outputPath;
            ProjectPath = csProjFile;
        }

        public void Prepare()
        {
            ProjectFile proj = ProjectFile.From(ProjectPath);
            if (proj.ApplyDocsharkConfiguration())
                proj.Save();

            // Build the project
            proj.BuildProject(ProjectPath, out targetAsmPath, out depAsmPaths);      
        }

        public void Load()
        {
            metadata = MetadataLoader.From(targetAsmPath, depAsmPaths);
            // Read in .xml documentation to be joined with member info        
            // Create an organized structure called a MetadataTree to represent .dll type structure
            Models = new ModelTree();

            /**
                * Add all types to MetadataTree
                */

            // Classes
            foreach (var item in metadata.Classes)
                Models.AddType(item.Key, item.Value);

            // Interfaces
            foreach (var item in metadata.Interfaces)
                Models.AddType(item.Key, item.Value);

            // Structs
            foreach (var item in metadata.Structs)
                Models.AddType(item.Key, item.Value);

            // Enumerations
            foreach (var item in metadata.Enumerations)
                Models.AddType(item.Key, item.Value);

            // Delegates
            foreach (var item in metadata.Delegates)
                Models.AddType(item.Key, item.Value);
        }

        public void Make()
        {
            Models.SaveModels(OutputPath);
        }    

        public void Dispose()
        {
            if (metadata != null)
            {
                metadata.Dispose();
                metadata = null;
            }
        }
    }
}
