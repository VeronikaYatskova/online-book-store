using Infrastructure.Extensions;
using BookStore.Application.Extensions;
using Serilog;
using BookStore.Application.Services.CloudServices.Amazon.Models;
using BookStore.Infrastructure.Consumers;
using BookStore.Application.Services.CloudServices.Azurite.Models;
using MassTransit;
using Microsoft.Extensions.Options;
using OnlineBookStore.Queues;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookStore.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLayers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationLayer();
            services.AddInfrastructureLayer(configuration);
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

        public static void AddCustomLogger(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Debug()
                .WriteTo.Console()
                // .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
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

                    configuration.Host(rabbitMqSettings.Host!, h =>
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

        public static void AddHangfireConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var hangfireConnectionString = configuration["ConnectionStrings:HangfireConnection"];
            
            services.AddHangfire(config => 
                config.UseSimpleAssemblyNameTypeSerializer()
                      .UseRecommendedSerializerSettings()
                      .UsePostgreSqlStorage(hangfireConnectionString));
            services.AddHangfireServer();
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string environment)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfig:Uri"]))    
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
                NumberOfReplicas = 2,
                NumberOfShards = 2,  
            };
        }
    }
}
