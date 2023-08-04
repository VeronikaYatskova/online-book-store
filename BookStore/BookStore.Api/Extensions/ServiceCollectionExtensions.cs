using Infrastructure.Extensions;
using BookStore.Application.Extensions;
using Serilog;
using Serilog.Events;
using BookStore.Application.Services.CloudServices.Amazon.Models;
using BookStore.Application.Consumers;
using Microsoft.Extensions.Options;

namespace BookStore.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLayers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationLayer();
            services.AddInfrastructureLayer(configuration);
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

        public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MinioConfiguration>(
                configuration.GetSection("MinioConfiguration"));

            services.Configure<RabbitMqSettings>(
                configuration.GetSection("RabbitMqConfig"));
            
            services.AddSingleton(sp =>
                 sp.GetRequiredService<IOptions<RabbitMqSettings>>().Value);
        }
    }
}
