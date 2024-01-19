using DotDocs.IO;
using DotDocs.Models;
using DotDocs.Models.Language;
using DotDocs.Render.Args;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Render
{
    public interface IRenderable
    {
        public RepositoryModel Model { get; set; }
        public ImmutableDictionary<string, ProjectModel> Projects { get; set; }
        public IOutputable Output { get; set; }

        public void Prepare(
            RepositoryModel model,
            ImmutableDictionary<string, ProjectModel> projects,
            IOutputable output)
        {
            Model = model;
            Projects = projects;
            Output = output;
        }

        public void Render()
        {
            foreach (var proj in Projects.Values)
                foreach (var type in proj.Assembly.Types)
                    RenderType(type);
        }

        public void RenderType(TypeModel model);
    }
}
