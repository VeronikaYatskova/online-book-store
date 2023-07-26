using Microsoft.Extensions.DependencyInjection;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Infrastructure.Persistance.Repositories;

namespace Profiles.Infrastructure.Extensions
{
    public static class ServiceCollectionExctension
    {
        public static void AddInfrastrucureLayer(this IServiceCollection services)
        {
            services.AddRepositories();
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>(); 
        }
    }
}
