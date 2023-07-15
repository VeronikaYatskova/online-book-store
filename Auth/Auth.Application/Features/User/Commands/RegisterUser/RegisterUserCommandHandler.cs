using Auth.Application.Abstractions.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Auth.Application.Exceptions;
using System.Security.Cryptography;
using System.Text;
using Auth.Application.Abstractions.Interfaces.Services;

namespace Auth.Application.Features.User.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IUserRepository userRepository;
        private readonly IUserRoleRepository userRoleRepository;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public RegisterUserCommandHandler(
            IUserRepository userRepository, 
            IUserRoleRepository userRoleRepository,
            ITokenService tokenService, 
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.userRoleRepository = userRoleRepository;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userData = request.request;
            if (userRepository.FindUserBy(u => u.Email == userData.Email) is not null)
            {
                throw new AlreadyExistsException(ExceptionMessages.UserAlreadyExistsMessage);
            }

            CreatePasswordHash(userData.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var userRoleId = new Guid(request.request.RoleId);
            var user = mapper.Map<Auth.Domain.Models.User>(userData);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Role = await userRoleRepository.GetUserRoleByIdAsync(userRoleId) ??
                throw new NotFoundException("No user with the given role.");
            user.RoleId = userRoleId;

            userRepository.AddUser(user);
            await userRepository.SaveUserChangesAsync();

            var token = tokenService.CreateToken(user);

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

        private bool VerifyPasswordHash(string password,
                                        byte[] passwordHash,
                                        byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
