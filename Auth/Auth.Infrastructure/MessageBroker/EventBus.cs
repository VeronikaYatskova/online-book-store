using Auth.Application.Abstractions.Interfaces.Services;
using Auth.Domain.Entities;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.MessageBroker
{
    public class EventBus : IEventBus
    {
        private readonly IBus _bus;
        private readonly IOptions<RabbitMqSettings> _options;

        public EventBus(IBus bus, IOptions<RabbitMqSettings> options)
        {
            _bus = bus;
            _options = options;
        }

        public async Task PublishAsync<T>(T message)
            where T : class
        {
            var uri = new Uri(_options.Value.Host + "/user-registered-event");
            var endpoint = await _bus.GetSendEndpoint(uri);
            await endpoint.Send(message);
        }
    }
}