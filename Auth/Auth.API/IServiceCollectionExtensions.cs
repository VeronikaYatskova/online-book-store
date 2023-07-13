using Auth.Infrastructure.Database.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Auth.API
{
    public static class IServiceCollectionExtensions
    {
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