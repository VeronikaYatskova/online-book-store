using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using OnlineBookStore.Messages.Models.Messages;
using Requests.BLL.DTOs.Requests;
using Requests.BLL.Services.Interfaces;

namespace Requests.BLL.Consumers
{
    public class UserCreatedConsumer : IConsumer<UserRegisteredMessage>
    {
        private readonly IUsersService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserCreatedConsumer> _logger;

        public UserCreatedConsumer(
            IUsersService userService, 
            IMapper mapper, 
            ILogger<UserCreatedConsumer> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserRegisteredMessage> context)
        {
            _logger.LogInformation("message recieved");

            var addRequestDto = _mapper.Map<AddUserRequest>(context.Message);

            await _userService.AddUserAsync(addRequestDto);
        }
    }
}
