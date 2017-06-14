using AzureTest.Helpers;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTest.Monitor
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
                    string message = ReadMessage("TestQueue/$DeadLetterQueue");

                    if (string.IsNullOrWhiteSpace(message)) break;
                    Console.WriteLine($"{DateTime.Now}: Message received: {message}");

                    Console.WriteLine($"{DateTime.Now}: Send e-mail");
                    SendEmail(message);
                }
            }

            sw.Stop();
            Console.WriteLine($"Done ({sw.Elapsed.TotalSeconds}) seconds");
            Console.ReadLine();
        }

        private static void SendEmail(string messageText)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add("@hotmail.co.uk");
            message.Subject = "Message in queue has expired";
            message.From = new System.Net.Mail.MailAddress("@hotmail.co.uk");
            message.Body = messageText;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.live.com");
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("@hotmail.co.uk", "");
            smtp.EnableSsl = true;
            smtp.Send(message);
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

            BrokeredMessage message = client.Receive();
            if (message == null) return string.Empty;
            string messageBody = message.GetBody<string>();

            //message.Complete();

            return messageBody;
        }

    }
}

