using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Profiles.Application.DTOs.General;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Application.Interfaces.Services;
using Profiles.Domain.Entities;
using Profiles.Domain.Models;

namespace Profiles.Application.Features.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory serviceScopeFactory; 
        private readonly IMapper mapper;
        private readonly ILogger<EventProcessor> logger;

        public EventProcessor(
            IServiceScopeFactory serviceScopeFactory, 
            IMapper mapper, 
            ILogger<EventProcessor> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.mapper = mapper;
            this.logger = logger;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.UserRegistered:
                    AddUser(message);
                    break;
                default:
                    break;
            }
        }

        private void AddUser(string user)
        {
            using var scope = serviceScopeFactory.CreateScope();

            var repo = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var userRegisteredDto = JsonSerializer.Deserialize<UserRegisteredDto>(user);

            var userEntity = mapper.Map<User>(userRegisteredDto);

            // check if the user exists
            repo.AddUserAsync(userEntity);
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType?.Event)
            {
                case EventTypes.UserRegisteredEvent:
                    logger.LogInformation("User register Event Detected.");
                    return EventType.UserRegistered;
                default:
                    logger.LogInformation("Could not determine the event type.");
                    return EventType.Undetermined;
            }
        }

        enum EventType
        {
            UserRegistered,
            Undetermined
        }
    }
}