using Microsoft.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTest.Helpers
{
    public class ServiceManagementHelper
    {
        public static Uri GetServiceUri()
        {
            Console.WriteLine($"Getting service bus URI...");
            Uri uri = ServiceBusEnvironment.CreateServiceUri("sb", "pcm-servicebustest", string.Empty);

            Console.WriteLine($"Service Bus URI: {uri.ToString()}");

            Console.WriteLine($"Connectivity: {ServiceBusEnvironment.SystemConnectivity.Mode.ToString()}");

            return uri;
        }

        public static TokenProvider GetTokenProvider(Uri uri)
        {
            Console.WriteLine($"Getting token...");
            TokenProvider tp = TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "JWh82nkstIAi4w5tW6MEj7GKQfoiZlwBYjHx9wfDqdA=");

            Console.WriteLine($"Token {tp.ToString()}");
            return tp;
        }

    }
}
