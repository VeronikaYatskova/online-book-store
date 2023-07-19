using Auth.Domain.Models;

namespace Auth.Application.Abstractions.Interfaces.Services
{
    public interface ITokenService
    {
        string CreateToken(AccountData accountData);
        Task<string> UpdateRefreshTokenAsync(AccountData accountData);
        Task SetRefreshTokenAsync(AccountData accountData);
        Task<AccountData?> GetAccountDataAsync();
    }
}
