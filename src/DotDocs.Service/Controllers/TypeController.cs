using DotDocs.Models.Language;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System;
using GDC = DotDocs.Models.GraphDatabaseConnection;
using System.Security.Cryptography;

namespace DotDocs.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TypeController : Controller
    {
        [HttpGet("types")]
        public IEnumerable<TypeModel> GetTypes()
        {
            var r = GDC.Client.Cypher
                        .Match("(p:Type)")
                        .Return<TypeModel>("(p)");

            return r.ResultsAsync.Result;
        }
    }
}
