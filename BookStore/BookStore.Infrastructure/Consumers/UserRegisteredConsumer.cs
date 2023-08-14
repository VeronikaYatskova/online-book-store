using AuthProfilesServices.Communication.Models;
using AutoMapper;
using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Domain.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BookStore.Infrastructure.Consumers
{
    public class UserRegisteredConsumer : IConsumer<UserRegisteredMessage>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserRegisteredConsumer> _logger;

        public UserRegisteredConsumer(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger<UserRegisteredConsumer> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserRegisteredMessage> context)
        {
            _logger.LogInformation("message received");
            var userEntity = _mapper.Map<User>(context.Message);

            await _unitOfWork.UsersRepository.CreateAsync(userEntity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
