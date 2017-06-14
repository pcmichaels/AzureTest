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

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

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

            sw.Stop();
            Console.WriteLine($"Done ({sw.Elapsed.TotalSeconds})");
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
                QueueDescription qd = new QueueDescription("TestQueue")
                {
                    DefaultMessageTimeToLive = new TimeSpan(0, 0, 30),
                    EnableDeadLetteringOnMessageExpiration = true
                };
                nm.CreateQueue(qd);
            }            

            return nm;
        }

        private static void AddNewMessage(string id, string messageBody, string queueName)
        {            
            BrokeredMessage message = new BrokeredMessage(messageBody)
            {
                MessageId = id 
            };

            QueueClient queueClient = QueueManagementHelper.GetQueueClient(queueName, false);               
            queueClient.Send(message);
        }
    }
}
