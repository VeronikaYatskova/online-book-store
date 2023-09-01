using Profiles.Infrastructure.Extensions;
using Profiles.Application.Extensions;
using Serilog;
using Serilog.Events;
using Profiles.Domain.Entities;
using Microsoft.Extensions.Options;
using MassTransit;
using Profiles.Infrastructure.Consumers;
using OnlineBookStore.Queues;

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

        public static void AddMassTransitConfig(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(busConfigurator =>
            {   
                busConfigurator.AddConsumer<UserRegisteredConsumer>();
                busConfigurator.AddConsumer<UserDeletedConsumer>();
                
                busConfigurator.UsingRabbitMq((context, configuration) => 
                {
                    var rabbitMqSettings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

                    configuration.Host(rabbitMqSettings.Host!, h =>
                    {
                        h.Username(rabbitMqSettings.UserName);
                        h.Password(rabbitMqSettings.Password);
                    });

                    configuration.ReceiveEndpoint(Queues.UserRegisteredQueue, c => 
                    {
                        c.ConfigureConsumer<UserRegisteredConsumer>(context);   
                    });

                    configuration.ReceiveEndpoint("user-deleted-queue", c => 
                    {
                        c.ConfigureConsumer<UserRegisteredConsumer>(context);   
                    });
                });
            });
        }
    }
}
