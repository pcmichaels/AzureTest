using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri uri = GetServiceUri();
            TokenProvider tokenProvider = GetTokenProvider(uri);

            CreateNewQueue(uri, tokenProvider);

            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static TokenProvider GetTokenProvider(Uri uri)
        {
            Console.WriteLine($"Getting token...");
            TokenProvider tp = TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "JWh82nkstIAi4w5tW6MEj7GKQfoiZlwBYjHx9wfDqdA=");                                                

            Console.WriteLine($"Token {tp.ToString()}");
            return tp;
        }

        private static void CreateNewQueue(Uri uri, TokenProvider tokenProvider)
        {
            Console.WriteLine($"Creating new queue...");
            NamespaceManager nm = new NamespaceManager(uri, tokenProvider);

            Console.WriteLine($"Created namespace manager for {nm.Address}");
            if (nm.QueueExists("TestQueue"))
            {
                Console.WriteLine("Queue already exists");
            }
            else
            {
                Console.WriteLine("Creating new queue");
                QueueDescription qd = nm.CreateQueue("TestQueue");
            }
        }

        private static Uri GetServiceUri()
        {
            Console.WriteLine($"Getting service bus URI...");
            //Uri uri = ServiceBusEnvironment.CreateAccessControlUri("pcm-servicebustest");
            Uri uri = ServiceBusEnvironment.CreateServiceUri("sb", "pcm-servicebustest", string.Empty);        

            Console.WriteLine($"Service Bus URI: {uri.ToString()}");

            Console.WriteLine($"Connectivity: {ServiceBusEnvironment.SystemConnectivity.Mode.ToString()}");

            return uri;
        }
    }
}
