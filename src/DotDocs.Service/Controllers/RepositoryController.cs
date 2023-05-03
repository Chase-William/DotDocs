using DotDocs.Models.Language;
using Microsoft.AspNetCore.Mvc;
using System;
using GDC = DotDocs.Models.GraphDatabaseConnection;

namespace DotDocs.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RepositoryController : Controller
    {
        [HttpPost("repository")]
        public IActionResult InsertRepository(string url)
        {
            var builder = DotDocs.New(url);

            builder.AddRepository();

            return Ok();
        }

        [HttpGet("repositories")]
        public IEnumerable<TypeModel> GetRepositories()
        {
            var r = GDC.Client.Cypher
                        .Match("(p:Repository)")
                        .Return<TypeModel>("(p)");

            return r.ResultsAsync.Result;
        }
    }
}
