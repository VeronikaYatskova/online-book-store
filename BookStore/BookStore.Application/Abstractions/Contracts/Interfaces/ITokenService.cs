namespace BookStore.Application.Abstractions.Contracts.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GetUserIdFromTokenAsync();
    }
}