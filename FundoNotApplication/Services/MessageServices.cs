using Azure.Messaging.ServiceBus;
using System.Text;

namespace FundoNoteswithAzure.Services
{
    public class MessageServices
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusSender _serviceBusSender;
        private readonly IConfiguration _configuration;

        public MessageServices(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = _configuration["AzureBus:ConnectionString"];
            string queueName = _configuration["AzureBus:Queue-Name"];


            _serviceBusClient = new ServiceBusClient(connectionString);
            _serviceBusSender = _serviceBusClient.CreateSender(queueName);
        }

        public async Task SendMessageToQueue(string email, string token)
        {
            byte[] encryptedToken = Encoding.UTF8.GetBytes(token);

            ServiceBusMessage message = new ServiceBusMessage(encryptedToken);

            message.To = email;
            message.Body = BinaryData.FromBytes(encryptedToken);

            await _serviceBusSender.SendMessageAsync(message);

        }
    }
}
