using System.Reflection;
using Auth.Application.Abstractions.Interfaces.Services;
using Auth.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddServices();
            services.AddHttpClient();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IHttpClientFactoryService, HttpClientFactoryService>();
        }
    }
}
