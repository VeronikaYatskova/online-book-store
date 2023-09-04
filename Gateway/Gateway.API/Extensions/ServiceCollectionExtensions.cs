namespace Gateway.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen();
            services.AddSwaggerForOcelot(configuration);
        }
    }
}
