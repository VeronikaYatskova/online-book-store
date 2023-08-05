using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Database.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; } = default!;
        public virtual DbSet<Author> Authors { get; set; } = default!;
        public virtual DbSet<Publisher> Publishers { get; set; } = default!;
        public virtual DbSet<AccountData> AccountsData { get; set; } = default!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(e => e.Id);
            modelBuilder.Entity<User>().HasOne(e => e.AccountData);
            modelBuilder.Entity<User>().HasIndex(e => new { e.AccountDataId }).IsUnique();
            modelBuilder.Entity<User>().Property(e => e.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Publisher>().HasKey(e => e.Id);
            modelBuilder.Entity<Publisher>().HasOne(e => e.AccountData);
            modelBuilder.Entity<Publisher>().HasIndex(e => new { e.AccountDataId }).IsUnique();
            modelBuilder.Entity<Publisher>().Property(e => e.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Author>().HasKey(e => e.Id);
            modelBuilder.Entity<Author>().HasOne(e => e.AccountData);
            modelBuilder.Entity<Author>().HasIndex(e => new { e.AccountDataId }).IsUnique();
            modelBuilder.Entity<Author>().Property(e => e.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<AccountData>().HasKey(e => e.Id);
            modelBuilder.Entity<AccountData>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<AccountData>().HasOne(e => e.Role).WithMany(r => r.AccountsData);

            modelBuilder.Entity<UserRole>().HasKey(e => e.Id);
            modelBuilder.Entity<UserRole>().Property(e => e.Id).ValueGeneratedOnAdd();
        }
    }
}
