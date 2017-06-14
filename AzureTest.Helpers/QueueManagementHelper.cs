using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTest.Helpers
{
    public class QueueManagementHelper
    {
        public static QueueClient GetQueueClient(string queueName, bool requireAck)
        {
            string connectionString = GetConnectionString();

            QueueClient queueClient = QueueClient.CreateFromConnectionString(connectionString, queueName, requireAck ? ReceiveMode.PeekLock : ReceiveMode.PeekLock);

            return queueClient;
        }

        public static string GetConnectionString()
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

    }
}
