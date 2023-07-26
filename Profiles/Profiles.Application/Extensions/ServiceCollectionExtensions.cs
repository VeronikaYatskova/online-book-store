using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Profiles.Application.Features.EventProcessing;
using Profiles.Application.Interfaces.Services;

namespace Profiles.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddSingleton<IEventProcessor, EventProcessor>(); 
        }                
    }
}
