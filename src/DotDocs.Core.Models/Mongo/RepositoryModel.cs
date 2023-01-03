using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Core.Models.Mongo
{
    public record class RepositoryModel
    {
        public string RepositoryName { get; set; }
        public string RepositoryUrl { get; set; }     
        public string CommitHash { get; set; }
    }
}
