using System.Security.Cryptography;
using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.Abstractions.Interfaces.Services;
using Auth.Application.DTOs.Request;
using Auth.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Auth.Application.Features.User.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        private readonly IValidator<LoginUserCommand> validator;

        public LoginUserCommandHandler(
            IUserRepository userRepository, 
            ITokenService tokenService, 
            IMapper mapper,
            IValidator<LoginUserCommand> validator)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request);

            var userData = request.request;

            var user = await userRepository.FindUserByAsync(u => u.Email == userData.Email);

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

        private void VerifyData(Domain.Models.User user, LoginUserRequest loginModel)
        {
            if (user!.Email != loginModel.Email)
            {
                throw new ArgumentException("Not found.");
            }

            if (!VerifyPasswordHash(loginModel.Password, user.PasswordHash!, user.PasswordSalt!))
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
