using Application.Abstractions.Contracts.Interfaces;
using Infrastructure.Persistance.DataContext;
using Infrastructure.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext(config);
            services.AddRepositories();
        }

        private static void AddDbContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContextPool<AppDbContext>(opt =>
                opt.UseLazyLoadingProxies()
                .UseNpgsql(config.GetConnectionString("PostgreSQLConnection")));
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IPublisherRepository, PublisherRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IAuthorsBooksRepository, AuthorsBooksRepository>();
            services.AddScoped<IFavoriteBooksRepository, FavoriteBooksRepository>();
        }
    }
}
