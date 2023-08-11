using Profiles.Infrastructure.Extensions;
using Profiles.Application.Extensions;
using Serilog;
using Serilog.Events;
using Profiles.Domain.Entities;
using Microsoft.Extensions.Options;

namespace Profiles.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddLayers(this IServiceCollection services)
        {
            services.AddInfrastrucureLayer();
            services.AddApplicationLayer();
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

        public static void AddOptionsConfiguration(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseSettings>(
                configuration.GetSection("ConnectionStrings"));

            services.Configure<RabbitMqSettings>(
                configuration.GetSection("RabbitMqConfig"));
        }
    }
}
