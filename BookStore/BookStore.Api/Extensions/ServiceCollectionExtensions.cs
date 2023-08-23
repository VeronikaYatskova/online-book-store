using Infrastructure.Extensions;
using BookStore.Application.Extensions;
using Serilog;
using Serilog.Events;
using BookStore.Application.Services.CloudServices.Amazon.Models;
using BookStore.Infrastructure.Consumers;
using BookStore.Application.Services.CloudServices.Azurite.Models;
using MassTransit;
using Microsoft.Extensions.Options;
using OnlineBookStore.Queues;

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
        
            services.Configure<BlobStorageSettings>(
                configuration.GetSection("BlobStorage"));
        }

        public static void AddMassTransitConfig(this IServiceCollection services)
        {
            services.AddMassTransit(busConfigurator =>
            {   
                busConfigurator.AddConsumer<UserRegisteredConsumer>();
                busConfigurator.AddConsumer<BookPublishingConsumer>();
                
                busConfigurator.UsingRabbitMq((context, configuration) => 
                {
                    var rabbitMqSettings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

                    configuration.Host(new Uri(rabbitMqSettings.Host!), h =>
                    {
                        h.Username(rabbitMqSettings.UserName);
                        h.Password(rabbitMqSettings.Password);
                    });

                    configuration.ReceiveEndpoint(Queues.UserRegisteredQueue, c => 
                    {
                        c.ConfigureConsumer<UserRegisteredConsumer>(context);   
                    });

                    configuration.ReceiveEndpoint(Queues.BookPublishedQueue, c =>
                    {
                        c.ConfigureConsumer<BookPublishingConsumer>(context);
                    });
                });
            });
        }
    }
}
