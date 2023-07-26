using System.Security.Cryptography;
using System.Text;
using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.Abstractions.Interfaces.Services;
using Auth.Application.DTOs.Request;
using Auth.Domain.Exceptions;
using Auth.Domain.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Auth.Application.Features.User.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IUserRepository userRepository;
        private readonly IUserRoleRepository userRoleRepository;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        private readonly IValidator<RegisterUserCommand> validator;
        private readonly IMessageBusClient messageBusClient;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            ITokenService tokenService, 
            IMapper mapper,
            IValidator<RegisterUserCommand> validator,
            IMessageBusClient messageBusClient)
        {
            this.userRepository = userRepository;
            this.userRoleRepository = userRoleRepository;
            this.tokenService = tokenService;
            this.mapper = mapper;
            this.validator = validator;
            this.messageBusClient = messageBusClient;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request);
            
            var userData = request.UserDataRequest;

            if (await userRepository.FindUserByAsync(u => u.Email == userData.Email) is not null)
            {
                throw new AlreadyExistsException(ExceptionMessages.UserAlreadyExistsMessage);
            }

            var userEntity = mapper.Map<Domain.Models.User>(userData); 
            
            CreatePasswordHash(userData.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var role = await userRoleRepository.GetUserRoleByNameAsync(request.Role) ??
                throw new NotFoundException(ExceptionMessages.RoleNotFoundMessage);

            userEntity.RoleId = role.Id;
            userEntity.PasswordHash = passwordHash;
            userEntity.PasswordSalt = passwordSalt;

            await userRepository.AddUserAsync(userEntity);
            await userRepository.SaveUserChangesAsync();

            var token = tokenService.CreateToken(userEntity);
            await tokenService.SetRefreshTokenAsync(userEntity);

            var userRegistersRequest = mapper.Map<UserRegisteredRequest>(userData);
            userRegistersRequest.Event = EventTypes.UserRegisteredEvent;

            messageBusClient.AddUserProfile(userRegistersRequest);

            return token;
        }

        private void CreatePasswordHash(string password,
                                        out byte[] passwordHash,
                                        out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
