using Docshark.Core.Models.Codebase.Members;
using Docshark.Core.Models.Codebase.Types;
using LoxSmoke.DocXml;
using System.IO;
using System.Reflection;
using System.Linq;
using Docshark.Core.Models.Codebase;
using Docshark.Core.Global.Types;
using Docshark.Core.Global;
using Docshark.Core.Models.Project;

namespace Docshark.Core
{
    /// <summary>
    /// Maps each type used in the target project.
    /// </summary>
    public class CodebaseManager
    {        
        MapperManager mapManager = new MapperManager(); 

        public CodebaseManager() { }

        public void Save(string outputPath)
            => mapManager.Save(outputPath);
        

        public void AddProjects(LocalProject rootProject)
        {            
            foreach (var proj in rootProject.LocalProjects)            
                AddProjects(proj);            
            mapManager.ProjectMap.AddProject(
                rootProject.ProjectName,
                rootProject.ProjectFileName,
                rootProject.ProjectDirectory,
                rootProject.ProjectPath,
                rootProject.LocalProjects.Select(p => p.ProjectName).ToArray(),
                rootProject.Assembly);
        }

        /// <summary>
        /// Adds given type and all dependent types to global type mapper if needed. 
        /// IMPORTANT: This method will perform a deep analysis of all types used in any manner by this type. 
        /// For example, types used in inheritance, encapsulated members, and even type arguments are analysed
        /// and added to the type mapper if needed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        public void AddType<T>(T type) where T : TypeMember<TypeInfo, TypeComments>
        {
            if (type is IMemberContainable containable) // Class, Struct, Interface
            {
                mapManager.TypeMap.AddType(type.Meta);
                AddMembers(containable);
                return;
            }
            else if (type is IFieldable fieldable) // Enum
            {
                mapManager.TypeMap.AddType(type.Meta);
                AddFields(fieldable.Fields);
                return;
            }
            // Delegate
            mapManager.TypeMap.AddType(type.Meta);

            void AddMembers(IMemberContainable model)
            {
                foreach (var mem in model.Properties)
                    mapManager.TypeMap.AddType(mem.Meta.PropertyType);
                AddFields(model.Fields);
                foreach (var mem in model.Events)
                    mapManager.TypeMap.AddType(mem.Meta.EventHandlerType);
                foreach (var mem in model.Methods)
                {
                    mapManager.TypeMap.AddType(mem.Meta.ReturnType);
                    if (mem.MethodType != null)
                        mapManager.TypeMap.AddType(mem.MethodType);
                    var parameters = mem.Meta.GetParameters();
                    foreach (var param in parameters)
                        mapManager.TypeMap.AddType(param.ParameterType);
                }
            }

            void AddFields(FieldModel[] fields)
            {
                foreach (var mem in fields)
                    mapManager.TypeMap.AddType(mem.Meta.FieldType);
            }
        }                      
    } 
}
