using DotDocs.Models.Language;
using Microsoft.AspNetCore.Mvc;
using System;
using GDC = DotDocs.Models.GraphDatabaseConnection;

namespace DotDocs.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : Controller
    {        
        [HttpGet("projects")]
        public IEnumerable<TypeModel> GetProjects()
        {
            var r = GDC.Client.Cypher
                        .Match("(p:Project)")
                        .Return<TypeModel>("(p)");

            return r.ResultsAsync.Result;
        }       
    }
}
