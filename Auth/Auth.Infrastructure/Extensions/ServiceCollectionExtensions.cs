using Auth.Application.Abstractions.Interfaces.Repositories;
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
            services.AddScoped<IAccountDataRepository, AccountDataRepository>();
            services.AddScoped<IPublisherRepository, PublisherRepository>(); 
            services.AddScoped<IAuthorRepository, AuthorRepository>();
        }
    }
}
