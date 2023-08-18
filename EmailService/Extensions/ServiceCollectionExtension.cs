using EmailService.Consumers;
using EmailService.Models;
using EmailService.Services;
using MassTransit;
using Microsoft.Extensions.Options;
using OnlineBookStore.Queues;
using Serilog;

namespace EmailService.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddModelOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailConfiguration>(configuration.GetSection("EmailConfig"));
            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqConfig"));
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, Services.EmailService>();
        }

        public static void AddMessageSender(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<RequestCreatedConsumer>();
                x.AddConsumer<RequestUpdateConsumer>();
                x.AddConsumer<EmailConfirmationConsumer>();

                x.UsingRabbitMq((context, config) =>
                {
                    var options = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

                    config.Host(options.Host, h => 
                    {
                       h.Username(options.UserName);
                       h.Password(options.Password); 
                    });

                    config.ReceiveEndpoint(Queues.RequestCreatedQueue, c => 
                    {
                        c.ConfigureConsumer<RequestCreatedConsumer>(context);   
                    });

                    config.ReceiveEndpoint(Queues.RequestUpdatedQueue, c =>
                    {
                        c.ConfigureConsumer<RequestUpdateConsumer>(context);
                    });

                    config.ReceiveEndpoint(Queues.EmailConfirmationQueue, c =>
                    {
                       c.ConfigureConsumer<EmailConfirmationConsumer>(context); 
                    });
                });
            });
        }

        public static void AddGloballLogger(this ILoggingBuilder loggingBuilder)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
            
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(Log.Logger);
        }
    }
}
