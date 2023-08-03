using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Persistance.Configurations
{
    public class BookAuthorEntityConfiguration : IEntityTypeConfiguration<BookAuthorEntity>
    {
        public void Configure(EntityTypeBuilder<BookAuthorEntity> builder)
        {
            builder.HasKey(ba => new { ba.AuthorGuid, ba.BookGuid});

            builder.HasOne(b => b.Book)
                   .WithMany(p => p.BookAuthors)
                   .HasForeignKey(b => b.BookGuid);

            builder.HasOne(b => b.Author)
                   .WithMany(b => b.BookAuthors)
                   .HasForeignKey(b => b.AuthorGuid);
        }
    }
}
