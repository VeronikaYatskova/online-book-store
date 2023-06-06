using Domain.Entities;

namespace Application.Abstractions.Contracts.Interfaces
{
    public interface IMessageProducer
    {
        public void SendingMessage(EmailData emailData, RabbitMqConnectionData connectionData);
    }
}