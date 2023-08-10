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
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SetModels();
        }

        public virtual DbSet<BookEntity> Books { get; set; } = default!;
        public virtual DbSet<PublisherEntity> Publishers { get; set; } = default!;
        public virtual DbSet<CategoryEntity> Categories { get; set; } = default!;
        public virtual DbSet<AuthorEntity> Authors { get; set; } = default!;
        public virtual DbSet<BookAuthorEntity> BooksAuthors { get; set; } = default!;
        public virtual DbSet<UserFavoriteBookEntity> FavoriteBooks { get; set; } = default!; 
        public virtual DbSet<User> Users { get; set; } = default!;
    }
}
