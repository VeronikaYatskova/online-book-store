using Requests.DAL.Models;
using Serilog;
using Serilog.Events;

namespace Requests.API.Extension
{
    public static class ServiceCollectionExtension
    {
        public static void AddApiLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions(configuration);
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

        private static void AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
        }
    }
}
