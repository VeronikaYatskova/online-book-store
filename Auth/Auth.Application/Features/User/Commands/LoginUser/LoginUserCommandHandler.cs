using System.Security.Cryptography;
using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.Abstractions.Interfaces.Services;
using Auth.Application.DTOs.Request;
using Auth.Application.Exceptions;
using Auth.Domain.Models;
using AutoMapper;
using MediatR;

namespace Auth.Application.Features.User.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository userRepository;
        private readonly IAccountDataRepository accountDataRepository;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public LoginUserCommandHandler(
            IUserRepository userRepository, 
            IAccountDataRepository accountDataRepository,
            ITokenService tokenService, 
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.accountDataRepository = accountDataRepository;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var userData = request.request;

            var user = accountDataRepository.FindAccountBy(u => u.Email == userData.Email);

            if (user is null)
            {
                throw new UserNotRegisteredException(ExceptionMessages.UserNotRegisteredMessage);
            }
            else
            {
                VerifyData(user, userData);

                var token = tokenService.CreateToken(user);
                await tokenService.SetRefreshTokenAsync(user);
                
                return token;
            }
        }

        private void VerifyData(AccountData accountData, LoginUserRequest loginModel)
        {
            if (accountData!.Email != loginModel.Email)
            {
                throw new ArgumentException("Not found.");
            }

            if (!VerifyPasswordHash(loginModel.Password, accountData.PasswordHash!, accountData.PasswordSalt!))
            {
                throw new ArgumentException("Wrong password");
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}