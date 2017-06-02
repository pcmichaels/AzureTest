using AzureTest.Helpers;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTestRead
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!InitialiseClient())
            {
                Console.WriteLine("Unable to initialise client");                
            }
            else
            {
                string message = ReadMessage("TestQueue");
                Console.WriteLine($"Message received: {message}");
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static bool InitialiseClient()
        {
            Uri uri = ServiceManagementHelper.GetServiceUri();
            TokenProvider tokenProvider = ServiceManagementHelper.GetTokenProvider(uri);

            NamespaceManager nm = new NamespaceManager(uri, tokenProvider);
            return nm.QueueExists("TestQueue");
        }

        private static string ReadMessage(string queueName)
        {
            QueueClient client = QueueManagementHelper.GetQueueClient(queueName);

            BrokeredMessage message = client.Receive();
            string messageBody = message.GetBody<string>();
            return messageBody;
        }
    }
}
