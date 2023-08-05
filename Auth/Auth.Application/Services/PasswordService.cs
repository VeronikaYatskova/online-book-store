using System.Security.Cryptography;
using System.Text;
using Auth.Application.Abstractions.Interfaces.Services;
using Auth.Application.DTOs.Request;
using Auth.Domain.Exceptions;
using Auth.Domain.Models;

namespace Auth.Application.Services
{
    public class PasswordService : IPasswordService
    {
        public void CreatePasswordHash(string password,
                                        out byte[] passwordHash,
                                        out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public void VerifyPassword(User user, LoginUserRequest loginModel)
        {
            if (!VerifyPasswordHash(loginModel.Password, user.PasswordHash!, user.PasswordSalt!))
            {
                throw new WrongPasswordException(ExceptionMessages.WrongPasswordMessage);
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
