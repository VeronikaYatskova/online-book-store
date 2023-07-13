namespace Auth.Application.Abstractions.Services
{
    public interface IHttpClientFactoryService
    {
        Task<string> Execute(string token);        
    }
}