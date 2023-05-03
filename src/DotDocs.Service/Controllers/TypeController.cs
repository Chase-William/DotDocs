using DotDocs.Models.Language;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System;
using GDC = DotDocs.Models.GraphDatabaseConnection;
using System.Security.Cryptography;
using Neo4jClient.Cypher;

// Neo4j docs used to build regex queries: https://neo4j.com/docs/cypher-manual/current/clauses/where/#case-insensitive-regular-expressions

namespace DotDocs.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TypeController : Controller
    {
        [HttpGet("types")]
        public IEnumerable<TypeModel> GetTypes(string? name)
        {
            ICypherFluentQuery<TypeModel> r;

            if (name != null)
            {
                r = GDC.Client.Cypher
                        .Match("(t:Type)")
                        .Where("t.name =~ '(?i).*' + $name + '.*'")
                        .WithParams(new
                        {
                            name
                        })
                        .Return<TypeModel>("(t)");

                return r.ResultsAsync.Result;
            }

            r = GDC.Client.Cypher
                        .Match("(t:Type)")
                        .Return<TypeModel>("(t)");

            return r.ResultsAsync.Result;
        }
    }
}
