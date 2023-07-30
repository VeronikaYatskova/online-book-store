using Auth.Application.Abstractions.Interfaces.Repositories;
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
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration _config)
        {
            AddDbContext(services, _config);
            AddRepositories(services);
        }

        private static void AddDbContext(this IServiceCollection services, IConfiguration _config)
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");       
            ArgumentNullException.ThrowIfNull(connectionString, nameof(connectionString));
            services.AddDbContext<AppDbContext>(options =>
                options.UseLazyLoadingProxies()
                       .UseNpgsql(connectionString));
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<User>, Repository<User>>();
            services.AddScoped<IRepository<UserRole>, Repository<UserRole>>();
        }
    }
}
