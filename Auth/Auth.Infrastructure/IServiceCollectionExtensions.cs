using Auth.Application.Abstractions.Repositories;
using Auth.Infrastructure.Database.DataContext;
using Auth.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration config)
        {
            AddDbContext(services, config);
            AddRepositories(services);
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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        }
    }
}
