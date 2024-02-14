using DotDocs.Models;
using Microsoft.Build.Construction;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Build
{    
    public static class BuildEx
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static ImmutableArray<AssemblyReflectInfo> GetAssemblies(this Microsoft.Build.Logging.StructuredLogger.Build build)
        {
            Debug.Assert(build.Succeeded is true);

            var assemblies = new Dictionary<string, AssemblyReflectInfo>();

            var eval = build.Children.Single(n => n is TimedNode t && t.Name == "Evaluation") as TimedNode;

            var mainBuild = build.FindLastChild<Project>();
            var target = mainBuild
                .FindFirstChild<Target>(c => c.Name == "FindReferenceAssembliesForReferences");
            var usedAssemblies = target.Children
                .Select(item => ((Item)((AddItem)item).FirstChild).Text)
                .ToImmutableArray();

            foreach (var item in eval.Children)
            {
                if (item is ProjectEvaluation proj && 
                    proj.ProjectFileExtension == ".csproj" &&
                    !assemblies.ContainsKey(proj.Name))
                {
                    assemblies.Add(proj.Name, proj.GetAssembly(usedAssemblies));                    
                }
            }

            return assemblies.Values.ToImmutableArray();
        }
    }
}
