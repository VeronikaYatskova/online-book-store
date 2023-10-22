using Auth.Application.DTOs.Request;
using Auth.Domain.Models;

namespace Auth.Application.Abstractions.Interfaces.Services
{
    public interface IPasswordService
    {
        void CreatePasswordHash(string password,
                                out byte[] passwordHash,
                                out byte[] passwordSalt);
        void VerifyPassword(User user, LoginUserRequest loginModel);
    }
}