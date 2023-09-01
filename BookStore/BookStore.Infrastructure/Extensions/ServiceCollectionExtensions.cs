using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Infrastructure.Persistance.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookStore.Infrastructure.Persistance.Repositories;
using BookStore.Infrastructure;
using PdfGenerator.Interfaces;
using PdfGenerator;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext(config);
            services.AddRepositories();
            services.AddScoped<ITemplateGenerator, TemplateGenerator>(); 
            services.AddScoped<IPdfGenerator, PdfGenerator.PdfGenerator>(); 
        }

        private static void AddDbContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContextPool<AppDbContext>(opt =>
                opt.UseNpgsql(config.GetConnectionString("PostgreSQLConnection")));
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        }
    }
}
