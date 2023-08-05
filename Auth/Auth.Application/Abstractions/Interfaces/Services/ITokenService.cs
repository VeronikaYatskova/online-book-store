using Auth.Domain.Models;

namespace Auth.Application.Abstractions.Interfaces.Services
{
    public interface ITokenService
    {
        string CreateToken(User accountData);
        Task<string> UpdateRefreshTokenAsync(User accountData);
        Task SetRefreshTokenAsync(User accountData);
        Task<User?> GetUserAsync();
    }
}
