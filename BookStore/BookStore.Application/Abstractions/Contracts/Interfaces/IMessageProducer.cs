using BookStore.Application.DTOs.Request;
using BookStore.Domain.Entities;

namespace BookStore.Application.Abstractions.Contracts.Interfaces
{
    public interface IMessageProducer
    {
        public void SendingMessage(EmailDataRequest emailData, RabbitMqConnectionData connectionData);
    }
}