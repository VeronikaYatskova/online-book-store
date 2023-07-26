namespace Profiles.Application.Interfaces.Services
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);        
    }
}