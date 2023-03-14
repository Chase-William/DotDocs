using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models
{
    public static class GraphDatabaseConnection
    {
        internal static IDriver driver;

        public static void Init(string uri, string user, string password)
        {
            driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }

        public static void Close()
            => driver?.Dispose();
    }
}
