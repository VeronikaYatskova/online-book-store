using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Application.Abstractions.Interfaces.Services;
using Auth.Domain.Models;
using Auth.Infrastructure.Database.DataContext;
using Auth.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext(config);
            services.AddRepositories();
        }

        private static void AddDbContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");       
            ArgumentNullException.ThrowIfNull(connectionString, nameof(connectionString));
            services.AddDbContext<AppDbContext>(options =>
                options.UseLazyLoadingProxies()
                       .UseNpgsql(connectionString));
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
