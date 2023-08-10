namespace Auth.Application.Abstractions.Interfaces.Services
{
    public interface IHttpClientFactoryService
    {
        Task<string> Execute(string token);        
    }
}