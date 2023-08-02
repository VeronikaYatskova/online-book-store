using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Persistance.DataContext
{
    public static class ModelBuilderExtenstions
    {
        public static void SetModels(this ModelBuilder modelBuilder)
        {
            modelBuilder.SetEntityToTable();
            modelBuilder.SetKeyAndPropertiesToModel();
        }

        private static void SetEntityToTable(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookEntity>().ToTable("Books");
            modelBuilder.Entity<BookAuthorEntity>().ToTable("AuthorBook");
            modelBuilder.Entity<CategoryEntity>().ToTable("Categories");
        }

        private static void SetKeyAndPropertiesToModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookEntity>().HasKey(b => b.BookGuid);
            modelBuilder.Entity<BookEntity>().Property(b => b.BookGuid).ValueGeneratedOnAdd();
            modelBuilder.Entity<BookEntity>().Property(b => b.BookName).IsRequired();
            modelBuilder.Entity<BookEntity>().Property(b => b.ISBN10).IsRequired();
            modelBuilder.Entity<BookEntity>().Property(b => b.ISBN13).IsRequired();
            modelBuilder.Entity<BookEntity>().Property(b => b.PagesCount).IsRequired();
            modelBuilder.Entity<BookEntity>().Property(b => b.PublisherGuid).IsRequired();
            modelBuilder.Entity<BookEntity>().Property(b => b.CategoryGuid).IsRequired();

            modelBuilder.Entity<BookEntity>().HasOne(b => b.Category).WithMany(p => p.Books).HasForeignKey(b => b.CategoryGuid);

            modelBuilder.Entity<BookAuthorEntity>().HasKey(ba => new { ba.AuthorGuid, ba.BookGuid});

            modelBuilder.Entity<CategoryEntity>().HasKey(c => c.CategoryGuid);

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<UserBookEntity>().HasKey(fb => new { fb.BookId, fb.UserId });
            modelBuilder.Entity<UserBookEntity>().Property(fb => fb.Id).ValueGeneratedOnAdd();
        }
    }
}
