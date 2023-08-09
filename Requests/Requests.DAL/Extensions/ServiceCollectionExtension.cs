using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Requests.DAL.Models;
using Requests.DAL.Repositories.Implementations;
using Requests.DAL.Repositories.Interfaces;

namespace Requests.DAL.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddDataAccessLayer(this IServiceCollection services)
        {
            services.AddRepositories();
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRequestsRepository, RequestsRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
