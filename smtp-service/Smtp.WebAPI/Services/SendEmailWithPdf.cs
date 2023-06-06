using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Smtp.WebAPI.Models;

namespace Smtp.WebAPI.Services
{
    public class SendEmailWithPdf
    {
        private readonly IConfiguration config;

        public SendEmailWithPdf(IConfiguration config)
        {
            this.config = config;
        }

        public void SendEmail(EmailInfo emailInfo)
        {
            var factory = new ConnectionFactory()
            {
                HostName = config["RabbitMqConfiguration:HostName"]!,
                UserName = config["RabbitMqConfiguration:UserName"]!,
                Password = config["RabbitMqConfiguration:Password"]!,
                VirtualHost = config["RabbitMqConfiguration:VirtualHost"]!,
            };

            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();
            channel.QueueDeclare("email-sending", durable: true, exclusive: true);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) => 
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Data is received.");
            };

            channel.BasicConsume("email-sending", true, consumer);
        }
    }
}