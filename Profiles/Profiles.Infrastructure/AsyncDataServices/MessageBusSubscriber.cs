using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Profiles.Application.Interfaces.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Profiles.Infrastructure.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly IEventProcessor eventProcessor;
        private readonly ILogger<MessageBusSubscriber> logger;
        private IConnection connection;
        private IModel channel;
        private string queueName;

        public MessageBusSubscriber(
            IConfiguration configuration, 
            IEventProcessor eventProcessor,
            ILogger<MessageBusSubscriber> logger)
        {
            this.configuration = configuration; 
            this.eventProcessor = eventProcessor;
            this.logger = logger;
            InitializeRabbitMq();
        }

        public override void Dispose()
        {
            if (channel.IsOpen)
            {
                channel.Close();
                connection.Close();
            }

            base.Dispose();
        }
        
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ModuleHandle, ea) =>
            {
                logger.LogInformation("Event Received");

                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                eventProcessor.ProcessEvent(notificationMessage);
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private void InitializeRabbitMq()
        {
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
            
            queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                exchange: "trigger",
                routingKey: "");

            logger.LogInformation("Listening on the Message Bus...");
            
            connection.ConnectionShutdown += RabbitMQConnectionShutdown;
        }

        private void RabbitMQConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            logger.LogInformation("RabbitMq connection shutdown.");
        }
    }
}
