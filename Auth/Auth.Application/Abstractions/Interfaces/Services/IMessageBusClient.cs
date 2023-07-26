using Auth.Application.DTOs.Request;

namespace Auth.Application.Abstractions.Interfaces.Services
{
    public interface IMessageBusClient
    {
        void AddUserProfile(UserRegisteredRequest newUser);
    }
}
