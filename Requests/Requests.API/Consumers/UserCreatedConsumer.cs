using AuthProfilesServices.Communication.Models;
using AutoMapper;
using MassTransit;
using Requests.BLL.DTOs.Requests;
using Requests.BLL.Services.Interfaces;

namespace Requests.API.Consumers
{
    public class UserCreatedConsumer : IConsumer<UserRegisteredMessage>
    {
        private readonly IUsersService _userService;
        private readonly IMapper _mapper;

        public UserCreatedConsumer(IUsersService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<UserRegisteredMessage> context)
        {
            var addRequestDto = _mapper.Map<AddUserRequest>(context.Message);

            await _userService.AddUserAsync(addRequestDto);
        }
    }
}
