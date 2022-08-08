﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Docshark.Core.Mapper.Codebase;
using Docshark.Core.Models.Codebase;
using Docshark.Core.Models.Codebase.Types;
using LoxSmoke.DocXml;

namespace Docshark.Core.Models.Project
{
    /// <summary>
    /// Represents a local project with a .csproj file.
    /// </summary>
    public sealed class LocalProject : Dependency, IDisposable
    {
        /// <summary>
        /// Just the project name with no extension.
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// Directory where the .csproj file resides.
        /// </summary>
        public string ProjectDirectory { get; set; }
        /// <summary>
        /// Just the file name.
        /// </summary>
        public string ProjectFileName { get; set; }
        /// <summary>
        /// Just the file extension.
        /// </summary>
        public string ProjectFileExt { get; set; }
        /// <summary>
        /// Entire path to file include name with extension.
        /// </summary>
        public string ProjectPath { get; set; }

        [JsonIgnore]
        public CodebaseMapper Codebase { get; set; }

        public LocalProject[] LocalProjects { get; set; }

        MetadataLoadContext mlc;
        bool isRendered;

        /// <summary>
        /// Disposes of unmanaged resources for this <see cref="LocalProject"/> only.
        /// Does not dispose of children projects in <see cref="LocalProjects"/>.
        /// </summary>
        public void Dispose()
            => mlc?.Dispose();

        /// <summary>
        /// Determines if a projectFile
        /// Uses a depth-first-search (DFS) approach.
        /// </summary>
        /// <param name="projectName">Name of project to find.</param>
        /// <returns>A reference to the <see cref="LocalProject"/> instance or null.</returns>
        public bool Exists(string projectFile)
        {
            foreach (var proj in LocalProjects)
            {
                if (proj.ProjectPath == projectFile) // base case
                    return true;
                return proj.Exists(projectFile);
            }
            return false;
        }

        public void Load(string[] assemblies, Action<TypeMember<TypeInfo, TypeComments>> getTypeCallback)
        {
            if (mlc != null)
                Dispose();
            mlc = new MetadataLoadContext(new PathAssemblyResolver(assemblies));
            Codebase = CodebaseMapper.Builder.Build(
                mlc.LoadFromAssemblyPath(AssemblyPath), 
                DocumentationPath,
                getTypeCallback);          
        }

        public void Save(string baseOutputPath)
        {
            foreach (var proj in LocalProjects)            
                proj.Save(baseOutputPath);                            
            if (!isRendered)
            {
                Codebase.SaveModels(Path.Combine(baseOutputPath, ProjectName));
                isRendered = true;
            }
        }
    }
}