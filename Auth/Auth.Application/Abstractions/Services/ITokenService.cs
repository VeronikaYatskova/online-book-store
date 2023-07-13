using Auth.Domain.Models;

namespace Auth.Application.Abstractions.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
        Task<string> UpdateRefreshToken(User user);
        Task SetRefreshToken(User user);
    }
}
