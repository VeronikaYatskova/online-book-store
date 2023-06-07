using Application.DTOs.Request;
using Domain.Entities;

namespace Application.Abstractions.Contracts.Interfaces
{
    public interface IMessageProducer
    {
        public void SendingMessage(EmailDataRequest emailData, RabbitMqConnectionData connectionData);
    }
}