using System.Security.Cryptography;
using System.Text;
using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.Abstractions.Interfaces.Services;
using Auth.Application.Exceptions;
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

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            ITokenService tokenService, 
            IMapper mapper,
            IValidator<RegisterUserCommand> validator)
        {
            this.userRepository = userRepository;
            this.userRoleRepository = userRoleRepository;
            this.tokenService = tokenService;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request);
            
            var userData = request.UserDataRequest;

            if (userRepository.FindUserBy(u => u.Email == userData.Email) is not null)
            {
                throw new AlreadyExistsException(ExceptionMessages.UserAlreadyExistsMessage);
            }

            var userEntity = mapper.Map<Domain.Models.User>(userData); 
            
            CreatePasswordHash(userData.Password, out byte[] passwordHash, out byte[] passwordSalt);

            userEntity.RoleId = await userRoleRepository.GetUserRoleIdByName(request.Role);
            userEntity.PasswordHash = passwordHash;
            userEntity.PasswordSalt = passwordSalt;

            userRepository.AddUser(userEntity);
            await userRepository.SaveUserChangesAsync();

            var token = tokenService.CreateToken(userEntity);
            await tokenService.SetRefreshTokenAsync(userEntity);

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
