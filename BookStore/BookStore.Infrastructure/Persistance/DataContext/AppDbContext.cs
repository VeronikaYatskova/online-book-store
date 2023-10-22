using System.Reflection;
using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Persistance.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<BookEntity> Books { get; set; } = default!;
        public DbSet<BookAuthorEntity> BooksAuthors { get; set; } = default!;
        public DbSet<UserBookEntity> UserBooks { get; set; } = default!; 
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<CategoryEntity> Categories { get; set; } = default!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
