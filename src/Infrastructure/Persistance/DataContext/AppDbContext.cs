using Domain.Entities;
using Infrastructure.Persistance.DataSeeding;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.DataContext
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

            // modelBuilder.ApplyConfiguration(new BooksConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new BookAuthorConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new PublisherConfiguration());

            modelBuilder.Entity<BookEntity>().ToTable("Books");
            modelBuilder.Entity<BookAuthorEntity>().ToTable("AuthorBook");
            modelBuilder.Entity<CategoryEntity>().ToTable("Categories");
            modelBuilder.Entity<PublisherEntity>().ToTable("Publishers");
            modelBuilder.Entity<AuthorEntity>().ToTable("Authors");

            modelBuilder.Entity<BookEntity>().HasKey(b => b.BookGuid);
            modelBuilder.Entity<BookEntity>().Property(b => b.BookGuid).ValueGeneratedOnAdd();
            modelBuilder.Entity<BookEntity>().Property(b => b.BookName).IsRequired();
            modelBuilder.Entity<BookEntity>().Property(b => b.ISBN10).IsRequired();
            modelBuilder.Entity<BookEntity>().Property(b => b.ISBN13).IsRequired();
            modelBuilder.Entity<BookEntity>().Property(b => b.PagesCount).IsRequired();
            modelBuilder.Entity<BookEntity>().Property(b => b.PublisherGuid).IsRequired();
            modelBuilder.Entity<BookEntity>().Property(b => b.CategoryGuid).IsRequired();

            modelBuilder.Entity<BookEntity>().HasOne(b => b.Publisher).WithMany(p => p.Books).HasForeignKey(b => b.PublisherGuid);
            modelBuilder.Entity<BookEntity>().HasOne(b => b.Category).WithMany(p => p.Books).HasForeignKey(b => b.CategoryGuid);

            modelBuilder.Entity<BookAuthorEntity>().HasKey(ba => new { ba.AuthorGuid, ba.BookGuid});

            modelBuilder.Entity<PublisherEntity>().HasKey(c => c.PublisherGuid);
            modelBuilder.Entity<PublisherEntity>().Property(c => c.PublisherGuid).ValueGeneratedOnAdd();
            modelBuilder.Entity<CategoryEntity>().HasKey(c => c.CategoryGuid);
            modelBuilder.Entity<AuthorEntity>().HasKey(c => c.AuthorGuid);

            modelBuilder.Entity<User>().HasKey(u => u.UserGuid);
            modelBuilder.Entity<User>().Property(u => u.UserGuid).ValueGeneratedOnAdd();

            modelBuilder.Entity<UserFavoriteBookEntity>().HasKey(fb => new { fb.BookId, fb.UserId });
            modelBuilder.Entity<UserFavoriteBookEntity>().Property(fb => fb.Id).ValueGeneratedOnAdd();
        }

        public virtual DbSet<BookEntity> Books { get; set; }
        public virtual DbSet<PublisherEntity> Publishers { get; set; }
        public virtual DbSet<CategoryEntity> Categories { get; set; }
        public virtual DbSet<AuthorEntity> Authors { get; set; }
        public virtual DbSet<BookAuthorEntity> BooksAuthors { get; set; }
        public virtual DbSet<UserFavoriteBookEntity> FavoriteBooks { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
