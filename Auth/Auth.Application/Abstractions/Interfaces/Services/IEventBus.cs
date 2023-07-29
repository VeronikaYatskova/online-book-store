namespace Auth.Application.Abstractions.Interfaces.Services
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T message) 
            where T : class;
    }
}
