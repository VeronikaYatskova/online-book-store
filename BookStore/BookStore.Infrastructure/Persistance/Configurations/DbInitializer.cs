using BookStore.Domain.Entities;
using BookStore.Infrastructure.Persistance.DataContext;

namespace BookStore.Infrastructure.Persistance.Configurations
{
    public class DbInitializer
    {
        public static async Task SeedDataAsync(AppDbContext appDbContext)
        {
            await appDbContext.Database.EnsureCreatedAsync();

            if (!appDbContext.Categories.Any())
            {
                await appDbContext.AddRangeAsync
                (
                    new CategoryEntity
                    {
                        CategoryName = "Fiction",
                    },
                    new CategoryEntity
                    {
                        CategoryName = "Non-Fiction",
                    },
                    new CategoryEntity
                    {
                        CategoryName = "Novel",
                    },
                    new CategoryEntity
                    {
                        CategoryName = "Romance",
                    },
                    new CategoryEntity
                    {
                        CategoryName = "Biography",
                    }
                );
            }

            await appDbContext.SaveChangesAsync();
        }
    }
}
