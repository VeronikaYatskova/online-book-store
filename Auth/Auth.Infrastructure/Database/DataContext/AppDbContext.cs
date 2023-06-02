using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Database.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(u => u.UserGuid);
            modelBuilder.Entity<User>().Property(u => u.UserGuid).ValueGeneratedOnAdd();
        }
        
        public virtual DbSet<User> Users { get; set; }
    }
}
