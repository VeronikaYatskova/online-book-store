using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Profiles.Application.Interfaces.Repositories;

namespace Profiles.Infrastructure.Consumers
{
    public class UserDeletedConsumer : IConsumer<UserDeletedMessage>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserRegisteredConsumer> _logger;

        public UserDeletedConsumer(
            IUserRepository userRepository,
            IMapper mapper, 
            ILogger<UserRegisteredConsumer> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserDeletedMessage> context)
        {
            _logger.LogInformation("message received");
            
            await _userRepository.DeleteUserAsync(Guid.Parse(context.Message.UserId));
        }
    }
}
