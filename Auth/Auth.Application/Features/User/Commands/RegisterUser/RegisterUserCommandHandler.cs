using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.Abstractions.Interfaces.Services;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Auth.Application.Features.User.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        private readonly IValidator<RegisterUserCommand> validator;

        public RegisterUserCommandHandler(
            IUserRepository userRepository, 
            ITokenService tokenService, 
            IMapper mapper,
            IValidator<RegisterUserCommand> validator)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request);
            
            var userData = request.UserDataRequest;
            var userEntity = mapper.Map<Domain.Models.User>(userData); 
            userEntity.AccountDataId = request.AccountData.Id;

            userRepository.AddUser(userEntity);
            await userRepository.SaveUserChangesAsync();

            var token = tokenService.CreateToken(request.AccountData);
            await tokenService.SetRefreshTokenAsync(request.AccountData);

            return token;
        }
    }
}
