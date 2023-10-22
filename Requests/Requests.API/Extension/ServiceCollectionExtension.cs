using System.Text;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineBookStore.Messages.Models.Messages;
using OnlineBookStore.Queues;
using Requests.BLL.Consumers;
using Requests.BLL.DTOs.General;
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
            services.AddMessageSender();
        }

        public static void AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(configuration.GetSection("AppSettings:SecretKey").Value!)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });
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

        private static void AddMessageSender(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserCreatedConsumer>();
                
                x.UsingRabbitMq((context, config) =>
                {
                    var options = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

                    config.Host(options.Host, h => 
                    {
                       h.Username(options.UserName);
                       h.Password(options.Password); 
                    });

                    config.ReceiveEndpoint(Queues.UserRegisteredQueue, c =>
                    {
                        c.ConfigureConsumer<UserCreatedConsumer>(context);
                    });
                });

                x.AddRequestClient<BookPublishingMessage>();
            });
        }

        private static void AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqConfig"));
            services.Configure<BlobStorageSettings>(configuration.GetSection("BlobStorage"));
        }
    }
}
