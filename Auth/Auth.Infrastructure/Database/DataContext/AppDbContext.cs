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

            modelBuilder.Entity<User>().HasKey(e => e.Id);
            modelBuilder.Entity<User>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>()
                .HasOne(e => e.Role)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.RoleId);
            modelBuilder.Entity<User>().HasIndex(e => e.Email).IsUnique();

            modelBuilder.Entity<UserRole>().HasKey(e => e.Id);
            modelBuilder.Entity<UserRole>().Property(e => e.Id).ValueGeneratedOnAdd();
        }
    }
}
