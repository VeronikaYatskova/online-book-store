using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.Abstractions.Interfaces.Services;
using Auth.Domain.Exceptions;
using Auth.Domain.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

using UserEntity = Auth.Domain.Models.User;

namespace Auth.Application.Features.User.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
        private readonly IValidator<RegisterUserCommand> _validator;

        public RegisterUserCommandHandler(
            IRepository<UserEntity> userRepository,
            IRepository<UserRole> userRoleRepository,
            ITokenService tokenService, 
            IPasswordService passwordService,
            IMapper mapper,
            IValidator<RegisterUserCommand> validator)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _tokenService = tokenService;
            _passwordService = passwordService;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request);
            
            var userData = request.UserDataRequest;

            if (await _userRepository.FindByConditionAsync(u => u.Email == userData.Email) is not null)
            {
                throw new AlreadyExistsException(ExceptionMessages.UserAlreadyExistsMessage);
            }

            var userEntity = _mapper.Map<Domain.Models.User>(userData); 
            
            _passwordService.CreatePasswordHash(userData.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var role = await _userRoleRepository.FindByConditionAsync(r => r.Name == request.Role) ??
                throw new NotFoundException(ExceptionMessages.RoleNotFoundMessage);

            userEntity.RoleId = role.Id;
            userEntity.PasswordHash = passwordHash;
            userEntity.PasswordSalt = passwordSalt;

            await _userRepository.CreateAsync(userEntity);
            await _userRepository.SaveChangesAsync();

            var token = _tokenService.CreateToken(userEntity);
            await _tokenService.SetRefreshTokenAsync(userEntity);

            var userRequest = _mapper.Map<UserRegisteredDto>(userEntity);

            return token;
        }
    }
}
