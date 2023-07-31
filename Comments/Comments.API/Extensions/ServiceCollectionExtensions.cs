using Comments.DAL.Entities;

namespace Comments.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApiLayer(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions(config);
        }

        private static void AddOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MongoDbSettings>(config.GetSection("MongoDbSettings"));
        }
    }
}
