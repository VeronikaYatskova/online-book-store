namespace Comments.DAL.Repositories.Interfaces
{
    public interface ICacheRepository
    {
        Task<T?> GetDataAsync<T>(string key);
        Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime);
        Task<object> RemoveDataAsync(string key);
    }
}
