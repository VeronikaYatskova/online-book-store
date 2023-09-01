using Comments.BLL.Consumers;
using Comments.BLL.DTOs.General;
using Comments.DAL.Entities;
using Comments.DAL.Repositories.Implementations;
using MassTransit;
using Microsoft.Extensions.Options;
using OnlineBookStore.Queues;
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

        public static void AddMassTransitConfig(this IServiceCollection services, IConfiguration config)
        {
            services.AddMassTransit(busConfigurator =>
            {   
                busConfigurator.AddConsumer<CommentAddedConsumer>();
                
                busConfigurator.UsingRabbitMq((context, configuration) => 
                {
                    var rabbitMqSettings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

                    configuration.Host(rabbitMqSettings.Host!, h =>
                    {
                        h.Username(rabbitMqSettings.UserName);
                        h.Password(rabbitMqSettings.Password);
                    });

                    configuration.ReceiveEndpoint(Queues.CommentAddedQueue, c => 
                    {
                        c.ConfigureConsumer<CommentAddedConsumer>(context);   
                    });
                });
            });
        }

        private static void AddOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MongoDbSettings>(config.GetSection("MongoDbSettings"));
            services.Configure<RabbitMqSettings>(config.GetSection("RabbitMqConfig"));
            services.Configure<CacheSettings>(config.GetSection("RedisConfig"));
        }
    }
}
