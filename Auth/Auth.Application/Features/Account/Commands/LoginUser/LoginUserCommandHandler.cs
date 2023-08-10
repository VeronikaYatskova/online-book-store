using System.Security.Cryptography;
using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.Abstractions.Interfaces.Services;
using Auth.Application.DTOs.Request;
using Auth.Application.Features.Account.Commands.LoginUser;
using Auth.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using MediatR;

using UserEntity = Auth.Domain.Models.User;

namespace Auth.Application.Features.User.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IRepository<UserEntity> _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IValidator<LoginUserCommand> _validator;
        private readonly IPasswordService _passwordService;

        public LoginUserCommandHandler(
            IRepository<UserEntity> userRepository, 
            ITokenService tokenService, 
            IMapper mapper,
            IValidator<LoginUserCommand> validator,
            IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _validator = validator;
            _passwordService = passwordService;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request);

            var userData = request.LoginUserData;

            var user = await _userRepository.FindByConditionAsync(u => u.Email == userData.Email);

            if (user is null)
            {
                throw new UserNotRegisteredException();
            }
            else
            {
                _passwordService.VerifyPassword(user, userData);

                var token = _tokenService.CreateToken(user);
                await _tokenService.SetRefreshTokenAsync(user);
                
                return token;
            }
        }
    }
}
