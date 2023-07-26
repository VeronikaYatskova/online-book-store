using System.Text;
using System.Text.Json;
using Auth.Application.Abstractions.Interfaces.Services;
using Auth.Application.DTOs.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Auth.Infrastructure.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration configuration;
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly ILogger<MessageBusClient> logger;

        public MessageBusClient(IConfiguration configuration, ILogger<MessageBusClient> logger)
        {
            this.configuration = configuration;
            this.logger = logger;

            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMqConfig:RabbitMqHost"],
                UserName = configuration["RabbitMqConfig:UserName"],
                Password = configuration["RabbitMqConfig:Password"],
                VirtualHost = configuration["RabbitMqConfig:VirtualHost"]
            };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

            connection.ConnectionShutdown += RabbitMQConnectionShutdown;
        }

        public void AddUserProfile(UserRegisteredRequest newUser)
        {
            var message = JsonSerializer.Serialize(newUser);

            if (connection.IsOpen)
            {
                logger.LogInformation("RabbitMq connection Open, sending message.");
                SendMessage(message);
            }
            else
            {
                logger.LogInformation("RabbitMQ connection is clised, not sending");
            }
        }

        public void Dispose()
        {
            logger.LogInformation("Message bus disposed.");
            
            if (channel.IsOpen)
            {
                channel.Close();
                connection.Close();
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
            logger.LogInformation($"Sending message {message}");
        }

        private void RabbitMQConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            logger.LogInformation("RabbitMq connection shutdown.");
        }
    }
}
