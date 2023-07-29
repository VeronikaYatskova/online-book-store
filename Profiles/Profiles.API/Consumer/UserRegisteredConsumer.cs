using MassTransit;
using Profiles.Application.DTOs.General;

namespace Profiles.API.Consumer
{
    public class UserRegisteredConsumer : IConsumer<UserRegisteredDto>
    {
        private readonly ILogger<UserRegisteredConsumer> _logger;

        public UserRegisteredConsumer(ILogger<UserRegisteredConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<UserRegisteredDto> context)
        {
            _logger.LogInformation("--> Message received.");
            var data = context.Message;

            return Task.CompletedTask;
        }
    }
}
