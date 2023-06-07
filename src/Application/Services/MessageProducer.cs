using System.Text;
using System.Text.Json;
using Application.Abstractions.Contracts.Interfaces;
using Application.DTOs.Request;
using AutoMapper;
using Domain.Entities;
using RabbitMQ.Client;

namespace Application.Services
{
    public class MessageProducer : IMessageProducer
    {
        private readonly IMapper mapper;

        public MessageProducer(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public void SendingMessage(EmailDataRequest emailData, RabbitMqConnectionData connectionData)
        {
            try
            {
                var factory = mapper.Map<ConnectionFactory>(connectionData);

                var connection = factory.CreateConnection();

                using var channel = connection.CreateModel();
                channel.QueueDeclare(connectionData.ChannelName, durable: true, exclusive: false);

                var jsonString = JsonSerializer.Serialize(emailData);
                var body = Encoding.UTF8.GetBytes(jsonString);

                channel.BasicPublish("", connectionData.ChannelName, body: body);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
