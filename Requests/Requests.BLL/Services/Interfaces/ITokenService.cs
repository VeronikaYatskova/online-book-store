namespace Requests.BLL.Services.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GetUserIdFromTokenAsync();
    }
}
