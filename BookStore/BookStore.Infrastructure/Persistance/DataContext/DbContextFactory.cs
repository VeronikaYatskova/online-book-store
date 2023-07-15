using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookStore.Infrastructure.Persistance.DataContext
{
    public class DbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            
            string connectionString = "User ID =admin;Password=admin;Server=localhost;Port=5432;Database=books; Integrated Security=true;Pooling=true;";
            optionsBuilder.UseNpgsql(connectionString, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
