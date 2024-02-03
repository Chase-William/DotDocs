using System.Reflection;

namespace DotDocs.Models
{
    public class ProjectModel : IDisposable
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public string Name { get; set; }
        
        public string SDK { get; set; } = string.Empty;
        
        public string TargetFramework { get; set; } = string.Empty;
        
        public Assembly Assembly { get; set; }

        public string DocumentationFilePath { get; set; }

        public List<ProjectModel> Projects { get; set; } = new();

        IDisposable _mlc;

        public ProjectModel(string name, Assembly assembly, string docPath, IDisposable mlc)
        {
            Logger.Debug("Params: [{nameLbl}: {nameValue}, {asmLbl}: {asmValue}, {docPathLbl}: {docPathValue}, {mlcLbl}: {mlcValue}]", 
                nameof(name), name,
                nameof(assembly), assembly.FullName,
                nameof(docPath), docPath,
                nameof(mlc), mlc);
            Name = name;
            Assembly = assembly;
            DocumentationFilePath = docPath;
            _mlc = mlc;
        }

        public void Dispose()
        {
            Logger.Trace("Cleaning up unmanaged {mlc}", _mlc.ToString());
            _mlc.Dispose();
        }
    }
}
