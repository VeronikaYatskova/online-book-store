using Auth.Domain.Models;

namespace Auth.Application.Abstractions.Interfaces.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
        Task<string> UpdateRefreshTokenAsync(User user);
        Task SetRefreshTokenAsync(User user);
        Task<User?> GetUserAsync();
    }
}
