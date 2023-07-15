using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Infrastructure.Persistance.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookStore.Infrastructure.Persistance.Repositories;

namespace Infrastructure.Extensions
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
