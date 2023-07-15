using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Database.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(u => u.UserGuid);
            modelBuilder.Entity<User>().Property(u => u.UserGuid).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().HasOne(u => u.Role).WithMany(u => u.Users);

            modelBuilder.Entity<UserRole>().HasKey(u => u.UserRoleGuid);
        }
        
        public virtual DbSet<User> Users { get; set; } = default!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = default!;
    }
}
