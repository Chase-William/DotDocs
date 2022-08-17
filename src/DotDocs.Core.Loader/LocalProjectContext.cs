using DotDocs.Core.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DotDocs.Core.Models.Language;
using System.Runtime.CompilerServices;

namespace DotDocs.Core.Loader
{
    internal sealed class LocalProjectContext : LocalProjectModel
    {
        MetadataLoadContext mlc;
        Assembly asm;

        public string AssemblyLoadPath { get; set; }

        public string DocumentationPath { get; set; }

        /// <summary>
        /// Disposes of unmanaged resources for this <see cref="LocalProject"/> only.
        /// Does not dispose of children projects in <see cref="LocalProjects"/>.
        /// </summary>
        public void Dispose()
            => mlc?.Dispose();

        public void Load(string[] assemblies)
        {
            if (mlc != null)
                Dispose();
            mlc = new MetadataLoadContext(new PathAssemblyResolver(assemblies));
            asm = mlc.LoadFromAssemblyPath(AssemblyLoadPath);

            /*
             * Do not add all types unless they are relevant to the custom types created by the developer(s)
             * or if they are public. All types used in some way by the developer(s')('s) types will be added
             * to the type list. That said, if a type is public and available to be used by external libraries,
             * ensure that type is accounted for regardless if it's compiler generated.
             */
            var typesOfInterest = asm.DefinedTypes
                .Where(type => !type
                    .CustomAttributes
                    .Any(attr => attr.AttributeType.Name == typeof(CompilerGeneratedAttribute).Name) || 
                    type.IsPublic && !type.IsNestedFamORAssem);

            foreach (var type in typesOfInterest)            
                DefinedTypes.Add(new TypeModel(type));            
        }

        //public void Save(string baseOutputPath, List<LocalProjectContext> saved)
        //{            
        //    foreach (var proj in LocalProjects)
        //        ((LocalProjectContext)proj).Save(baseOutputPath, saved);
        //    if (!saved.Contains(this))
        //    {
        //        // Perform save logic
        //        // Codebase.SaveModels(Path.Combine(baseOutputPath, ProjectName));
        //        saved.Add(this);
        //    }
        //}
    }
}
