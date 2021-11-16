using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KEDB.Audit
{
    public class AzureEventHubAuditLog : IAuditLog
    {
        //private EventHubProducerClient eventHubClient;
        private readonly JsonSerializerOptions serializationOptions = new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        /*
        public AzureEventHubAuditLog(string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            eventHubClient = CreateEventHubClient(connectionString);
        }

        private static EventHubProducerClient CreateEventHubClient(string connectionString)
        {
            return new EventHubProducerClient(connectionString, new EventHubProducerClientOptions
            {
                ConnectionOptions = new EventHubConnectionOptions
                {
                    //Use the websockets transport type to ensure that traffic runs
                    //over port 443 and we can develop within an UFST office setting.
                    TransportType = EventHubsTransportType.AmqpWebSockets
                }
            });
        }
        */
        public async Task Log(UserAction userAction)
        {
            if (userAction == null)
            {
                throw new ArgumentNullException(nameof(userAction));
            }

            var json = JsonSerializer.Serialize(userAction, serializationOptions);

           /* var data = new EventData(System.Text.Encoding.UTF8.GetBytes(json));
            await eventHubClient.SendAsync(new[] { data }); */
        }
    }


}
