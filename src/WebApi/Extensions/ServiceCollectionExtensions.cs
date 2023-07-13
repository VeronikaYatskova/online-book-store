using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Application.Extensions;
using Serilog;
using Serilog.Events;

namespace WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLayers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationLayer();
            services.AddInfrastructureLayer(configuration);
        }

        public static void AddCustomLogger(this WebApplicationBuilder builder)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();

            builder.Logging.AddSerilog(logger);
        }

        public static void MigrateDatabase<T>(this WebApplication app) where T : DbContext
        {
            using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                var db = serviceScope.ServiceProvider.GetRequiredService<T>().Database;
                while (!db.CanConnect())
                {
                    logger.LogInformation("Database not ready yet; waiting...");
                    Thread.Sleep(1000);
                }
    
                db.Migrate();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
