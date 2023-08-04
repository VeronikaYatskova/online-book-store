using BookStore.Infrastructure.Persistance.Configurations;
using BookStore.Infrastructure.Persistance.DataContext;

namespace BookStore.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public async static Task<IApplicationBuilder> SeedDataToDbAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            
            var context = services.GetRequiredService<AppDbContext>();
            await DbInitializer.SeedDataAsync(context);

            return app;
        }
    }
}
