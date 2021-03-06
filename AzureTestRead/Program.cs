﻿using AzureTest.Helpers;
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
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            if (!InitialiseClient())
            {
                Console.WriteLine("Unable to initialise client");                
            }
            else
            {
                while (true)
                {
                    string message = ReadMessage("TestQueue");

                    if (string.IsNullOrWhiteSpace(message)) break;
                    Console.WriteLine($"{DateTime.Now}: Message received: {message}");
                }
            }

            sw.Stop();
            Console.WriteLine($"Done ({sw.Elapsed.TotalSeconds}) seconds");
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
            QueueClient client = QueueManagementHelper.GetQueueClient(queueName, true);

            BrokeredMessage message = client.Receive(new TimeSpan(0, 0, 30));            
            if (message == null) return string.Empty;
            string messageBody = message.GetBody<string>();

            message.Complete();

            return messageBody;
        }
    }
}
