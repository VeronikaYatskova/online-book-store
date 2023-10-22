using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Auth.Infrastructure.Extensions;
using Auth.Application.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Auth.Domain.Models;
using MassTransit;
using Microsoft.Extensions.Options;
using Auth.Domain.Entities;

namespace Auth.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
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

        public static void AddCustomSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standart Authorization header using the Bearer scheme(\"bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }

        public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GoogleCredentials>(
                configuration.GetSection("GoogleAuth")
            );

            services.Configure<AppSettings>(
                configuration.GetSection("AppSettings")
            );
        }

        public static void AddMassTransitConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, config) =>
                {
                    var options = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

                    config.Host(options.Host, h =>
                    {
                        h.Username(options.UserName);
                        h.Password(options.Password);
                    });
                });
            });
        }

        public static void AddLayers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructureLayer(configuration);
            services.AddApplicationLayer();
        }
    }
}
