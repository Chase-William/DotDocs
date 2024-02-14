using DotDocs.Models;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Build
{    
    public static class ProjectEvaluationEx
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        #region Binlog Variables
        const string PROJECT_NAME = "ProjectName";
        const string PROJECT_DIR = "ProjectDir";
        const string PROJECT_FILE_NAME = "ProjectFileName";
        const string PROJECT_PATH = "ProjectPath";
        const string TARGET_FILE_NAME = "TargetName";
        const string ASSEMBLY_NAME = "AssemblyName";
        const string TARGET_PATH = "TargetPath";
        const string DOCUMENTATION_FILE = "DocumentationFile";
        #endregion

        public static AssemblyReflectInfo GetAssembly(this ProjectEvaluation eval, ImmutableArray<string> dependencies)
        {
            var props = eval.FindChild<Folder>("Properties");

            var properties = new Dictionary<string, string>();

            foreach (var item in props.Children)
            {
                if (item is NameValueNode value && IsDesired(value))
                {
                    properties[value.Name] = value.Value;
                }
            }

            var projectFolder = eval.SourceFilePath[..eval.SourceFilePath.LastIndexOf('\\')];

            var asmPath = properties[TARGET_PATH];
            var docPath = Path.Combine(projectFolder, properties[DOCUMENTATION_FILE]);

            var info = new AssemblyReflectInfo(
                docPath,
                asmPath,
                new MetadataLoadContext(new PathAssemblyResolver(dependencies)));

            return info;

            // Determines if the NameValueNode should be kept, moved here for readability
            static bool IsDesired(NameValueNode vNode)
            {
                return vNode.Name switch
                {
                    PROJECT_DIR or
                    PROJECT_FILE_NAME or
                    PROJECT_PATH or
                    TARGET_FILE_NAME or
                    TARGET_PATH or
                    PROJECT_NAME or
                    ASSEMBLY_NAME or
                    DOCUMENTATION_FILE => true,
                    _ => false,
                };
            }
        }
    }
}
