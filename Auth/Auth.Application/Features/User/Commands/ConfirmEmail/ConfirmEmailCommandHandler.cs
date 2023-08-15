using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.Abstractions.Interfaces.Services;
using Auth.Domain.Exceptions;
using AuthProfilesServices.Communication.Models;
using AutoMapper;
using MassTransit;
using MediatR;

using UserEntity = Auth.Domain.Models.User;

namespace Auth.Application.Features.User.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, string>
    {
        private readonly IRepository<UserEntity> _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public ConfirmEmailCommandHandler(
            IRepository<UserEntity> userRepository,
            ITokenService tokenService,
            IMapper mapper,
            IPublishEndpoint publishEndpoint)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<string> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByConditionAsync(x => x.Email == request.Email) ?? 
                throw new EmailConfirmationException();
            
            user.EmailConfirmed = true;

            await _userRepository.SaveChangesAsync();

            var token = _tokenService.CreateToken(user);
            await _tokenService.SetRefreshTokenAsync(user);

            var userMessage = _mapper.Map<UserRegisteredMessage>(user);
            
            await _publishEndpoint.Publish(userMessage);

            return token;
        }
    }
}
