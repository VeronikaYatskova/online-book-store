using Auth.Application.Abstractions.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Auth.Application.Exceptions;
using System.Security.Cryptography;
using System.Text;
using Auth.Domain.Models;
using FluentValidation;

namespace Auth.Application.Features.Account.Commands.RegisterAccount
{
    public class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountCommand, AccountData>
    {
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IAccountDataRepository accountDataRepository;
        private readonly IMapper mapper;
        private readonly IValidator<RegisterAccountCommand> validator;

        public RegisterAccountCommandHandler(
            IUserRoleRepository userRoleRepository,
            IAccountDataRepository accountDataRepository,
            IMapper mapper,
            IValidator<RegisterAccountCommand> validator)
        {
            this.userRoleRepository = userRoleRepository;
            this.accountDataRepository = accountDataRepository;
            this.mapper = mapper;
            this.validator = validator;
        }
        
        public async Task<AccountData> Handle(RegisterAccountCommand registerUserDataRequest, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(registerUserDataRequest);

            var userData = registerUserDataRequest.RegisterAccountDataRequest;

            if (accountDataRepository.FindAccountBy(u => u.Email == userData.Email) is not null)
            {
                throw new AlreadyExistsException(ExceptionMessages.UserAlreadyExistsMessage);
            }

            CreatePasswordHash(userData.Password, out byte[] passwordHash, out byte[] passwordSalt);
            
            var userRoleId = await userRoleRepository.GetUserRoleIdByName(registerUserDataRequest.Role);
            var userAccountData = mapper.Map<AccountData>(userData);

            userAccountData.PasswordHash = passwordHash;
            userAccountData.PasswordSalt = passwordSalt;
            userAccountData.Role = await userRoleRepository.GetUserRoleByIdAsync(userRoleId) ??
                throw new NotFoundException("No user with the given role.");
            userAccountData.RoleId = userRoleId;

            accountDataRepository.AddAccount(userAccountData);
            await accountDataRepository.SaveChangesAsync();

            return userAccountData;
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
