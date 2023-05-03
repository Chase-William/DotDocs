using DotDocs.Models.Language;
using Microsoft.AspNetCore.Mvc;
using GDC = DotDocs.Models.GraphDatabaseConnection;

namespace DotDocs.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssemblyController : Controller
    {
        [HttpGet("assemblies")]
        public IEnumerable<TypeModel> GetAssemblies()
        {
            var r = GDC.Client.Cypher
                        .Match("(p:Assembly)")
                        .Return<TypeModel>("(p)");

            return r.ResultsAsync.Result;
        }
    }
}
