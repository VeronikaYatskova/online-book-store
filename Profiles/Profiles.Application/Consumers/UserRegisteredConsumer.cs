using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using OnlineBookStore.Messages;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Entities;

namespace Profiles.Application.Consumers
{
    public class UserRegisteredConsumer : IConsumer<UserRegisteredMessage>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserRegisteredConsumer> _logger;

        public UserRegisteredConsumer(
            IUserRepository userRepository,
            IMapper mapper, 
            ILogger<UserRegisteredConsumer> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserRegisteredMessage> context)
        {
            _logger.LogInformation("message received");
            
            var receivedMessage = _mapper.Map<User>(context.Message);
            
            await _userRepository.AddUserAsync(receivedMessage);
        }
    }
}
