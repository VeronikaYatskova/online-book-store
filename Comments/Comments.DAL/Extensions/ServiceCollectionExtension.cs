using Comments.DAL.Repositories.Implementations;
using Comments.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Comments.DAL.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration config)
        {
            services.AddRepositories();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICommentsRepository, CommentsRepository>();
            services.AddScoped<ICacheRepository, CacheRepository>();
        }
    }
}
