using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Gateway.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen();
            services.AddSwaggerForOcelot(configuration);
        }

        public static void ConfigureAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = "gatewayApi",
                        ValidIssuer = "gatewayApi",
                    };
                });
        }
    }
}
