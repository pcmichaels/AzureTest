using AzureTest.Helpers;
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
            Uri uri = ServiceManagementHelper.GetServiceUri();
            TokenProvider tokenProvider = ServiceManagementHelper.GetTokenProvider(uri);

            CreateNewQueue(uri, tokenProvider);

            Console.WriteLine("Size of message to send: ");
            string messageSizeStr = Console.ReadLine();
            string message = string.Empty;

            if (int.TryParse(messageSizeStr, out int messageSizeInt))
                message = new string('A', messageSizeInt);
            else
                throw new Exception("Must enter integer");

            Console.WriteLine("Number of times: ");
            string iterations = Console.ReadLine();
            if (int.TryParse(iterations, out int iterationsInt))
            {
                for (int i = 1; i <= iterationsInt; i++)
                {
                    string messageToSend = $"{message}{i}";
                    Console.WriteLine($"{DateTime.Now} Adding message {messageToSend} {i}");
                    AddNewMessage("1", messageToSend, "TestQueue");
                }
            }
            else
            {
                throw new Exception("Not a number");
            }
            /*
            while (true)
            {
                Console.Write("Enter message, or press [Enter] to exit: ");
                string message = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(message)) break;

                AddNewMessage("1", message, "TestQueue");
            }
            */

            Console.WriteLine("Done");
            Console.ReadLine();
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

            QueueClient queueClient = QueueManagementHelper.GetQueueClient(queueName);   
            queueClient.Send(message);
        }
    }
}
