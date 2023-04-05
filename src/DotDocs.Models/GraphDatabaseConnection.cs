using Neo4j.Driver;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models
{
    public static class GraphDatabaseConnection
    {
        internal static BoltGraphClient Client { get; private set; }

        public static void Init(string uri, string user, string password)
        {
            //var driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password), (b) =>
            //{
            //    b.WithEncryptionLevel(EncryptionLevel.None);               
            //});

            try
            {
                Client = new BoltGraphClient(uri, user, password);
                var connectTask = Client.ConnectAsync();
                connectTask.Wait();
                
                //var session = driver.AsyncSession();

                //Client = new BoltGraphClient(uri, user, password);                
                // Client = new BoltGraphClient(uri, user, password, encryptionLevel: Neo4j.Driver.EncryptionLevel.None);            
                //await Client.ConnectAsync(await NeoServerConfiguration.GetConfigurationAsync(new Uri(uri), user, password));
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }            
        }


        //public static void Close()
        //    => driver?.Dispose();
    }
}
