using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

            AddNewMessage("1", "test", "TestQueue");

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

        private static NamespaceManager CreateNewQueue(Uri uri, TokenProvider tokenProvider)
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

            return nm;
        }

        private static void AddNewMessage(string id, string messageBody, string queueName)
        {
            BrokeredMessage message = new BrokeredMessage(messageBody)
            {
                MessageId = id
            };

            string connectionString = GetConnectionString();
            
            QueueClient queueClient = QueueClient.CreateFromConnectionString(connectionString, queueName);
            queueClient.Send(message);
        }
        
        private static string GetConnectionString()
        {
            return "Endpoint=sb://pcm-servicebustest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=JWh82nkstIAi4w5tW6MEj7GKQfoiZlwBYjHx9wfDqdA=";                    
            
            /*
            string Url = string.Format("https://management.core.windows.net/{0}/services/servicebus/namespaces/{1}/ConnectionDetails", subscriptionId, namespaceId);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Url);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(Url).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {                
                string result = response.Content.ReadAsStringAsync().Result;

                return result;
            }

            throw new Exception("Could not get connection string");
            */
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
