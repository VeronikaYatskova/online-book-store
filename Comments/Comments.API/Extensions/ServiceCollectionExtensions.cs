using System.Text;
using Comments.BLL.Consumers;
using Comments.BLL.DTOs.General;
using Comments.DAL.Entities;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
