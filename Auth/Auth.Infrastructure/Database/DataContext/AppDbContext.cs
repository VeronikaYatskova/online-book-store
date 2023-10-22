using System.Reflection;
using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Database.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; } = default!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = default!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
