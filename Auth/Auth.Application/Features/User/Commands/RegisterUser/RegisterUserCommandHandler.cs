using Auth.Application.Abstractions.Repositories;
using AutoMapper;
using MediatR;
using Auth.Domain.Exceptions;
using System.Security.Cryptography;
using System.Text;
using Auth.Application.Abstractions.Services;

namespace Auth.Application.Features.User.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public RegisterUserCommandHandler(IUserRepository userRepository, ITokenService tokenService, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userData = request.request;
            if (userRepository.FindUserBy(u => u.Email == userData.Email) is not null)
            {
                throw Exceptions.UserAlreadyExists;
            }

            CreatePasswordHash(userData.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = mapper.Map<Auth.Domain.Models.User>(userData);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var token = tokenService.CreateToken(user, request.secretKey);

            userRepository.AddUser(user);
            await userRepository.SaveUserChangesAsync();

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