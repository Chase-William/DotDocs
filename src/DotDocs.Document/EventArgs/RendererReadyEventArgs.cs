using DotDocs.IO;
using DotDocs.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Render.Args
{
    public class RendererReadyEventArgs : EventArgs
    {
        public RepositoryModel RepoModel { get; init; }

        public ImmutableDictionary<string, ProjectModel> Projects { get; init; }

        public IOutputable Output { get; init; }

        public RendererReadyEventArgs(
            RepositoryModel model,
            ImmutableDictionary<string, ProjectModel> projects,
            IOutputable output
        ) {
            RepoModel = model;
            Projects = projects;
            Output = output;
        }
    }
}
