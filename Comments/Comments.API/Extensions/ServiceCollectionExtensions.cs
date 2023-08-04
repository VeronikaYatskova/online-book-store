using Comments.DAL.Entities;
using Serilog;
using Serilog.Events;

namespace Comments.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApiLayer(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions(config);
        }

        public static void AddCustomLogger(this ILoggingBuilder loggingBuilder)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();

            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(logger);
        }

        private static void AddOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MongoDbSettings>(config.GetSection("MongoDbSettings"));
        }
    }
}
